using System;
using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Lexer.Abstractions;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;
using Sx.Lexer;
using Sx.Lexer.Abstractions;
using Sxc.Compiler.Parser.Abstractions;

namespace Sx.Compiler.Parser
{
    public class SyntaxParser
    {
        private readonly ITokenizer _tokenizer;
        private readonly IErrorSink _errorSink;
        private ISourceFile _sourceFile;
        private ParserOptions _options;
        private bool _error;
        private int _index;
        private IEnumerable<IToken> _tokens;

        public IErrorSink ErrorSink => _errorSink;
        private IToken _current => _tokens.ElementAtOrDefault(_index) ?? _tokens.Last();
        private IToken _last => Peek(-1);
        private IToken _next => Peek(1);

        public CompilationUnit Parse(ISourceFile sourceFile)
        {
            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            _sourceFile = sourceFile;
            _tokens = _tokenizer.Tokenize(sourceFile).Where(t => !t.IsTrivia()).ToList();
            _index = 0;

            if (_errorSink.HasErrors)
                return null;

            try
            {
                return new CompilationUnit(new[] { ParseInternal() });
            }
            catch (SyntaxException ex)
            {
                return null;
            }
        }
        public CompilationUnit Parse(IEnumerable<ISourceFile> sourceFiles)
        {
            if (sourceFiles == null)
                throw new ArgumentNullException(nameof(sourceFiles));

            var children = new List<SyntaxNode>();

            // TODO(Dan): Do this in parallel!
            foreach (var file in sourceFiles)
            {
                _sourceFile = file;
                _tokens = _tokenizer.Tokenize(file).Where(t => !t.IsTrivia()).ToList();
                _index = 0;

                if (_errorSink.HasErrors)
                    return null;

                try
                {
                    children.Add(ParseInternal());
                }
                catch (SyntaxException ex)
                {

                }
            }

            return new CompilationUnit(children);
        }

        private SyntaxNode ParseInternal()
        {
            return ParseDocument();
        }
        private bool IsMakingProgress(int lastTokenPosition)
        {
            if (_index > lastTokenPosition)
                return true;

            return false;
        }
        private IToken Peek(int ahead)
        {
            return _tokens.ElementAtOrDefault(_index + ahead) ?? _tokens.Last();
        }
        private void Advance()
        {
            _index++;
        }

        #region Take Tokens

        private IToken Take()
        {
            var token = _current;

            Advance();

            return token;
        }
        private IToken Take(TokenType type)
        {
            if (_current.TokenType != type)
                throw UnexpectedToken(type);

            return Take();
        }
        private IToken Take(string contextualKeyword)
        {
            if (_current.TokenType != TokenType.Identifier && _current.Value != contextualKeyword)
            {
                throw UnexpectedToken(contextualKeyword);
            }

            return Take();
        }
        private IToken TakeKeyword(string keyword)
        {
            if (_current.TokenType != TokenType.Keyword && _current.Value != keyword)
                throw UnexpectedToken(keyword);

            return Take();
        }
        private IToken TakeSemicolon()
        {
            return Take(TokenType.Semicolon);
        }

        #endregion

        #region Make

        private void MakeBlock(Action action, TokenType openType = TokenType.LeftBracket, TokenType closeType = TokenType.RightBracket)
        {
            Take(openType);

            MakeStatement(action, closeType);
        }
        private void MakeStatement(Action action, TokenType closeType = TokenType.Semicolon)
        {
            try
            {
                var startIndex = _index;

                while (_current.TokenType != closeType && _current.TokenType != TokenType.EOF)
                {
                    action();

                    if (!IsMakingProgress(startIndex))
                        throw SyntaxError(Severity.Error, $"Unexpected '{_current.Value}'", CreateSpan(_current.SourceFilePart.Start));

                    startIndex = _index;
                }
            }
            catch (SyntaxException)
            {
                while (_current.TokenType != closeType && _current.TokenType != TokenType.EOF)
                {
                    Take();
                }
            }
            finally
            {
                if (_error)
                {
                    if (_last.TokenType == closeType)
                    {
                        _index--;
                    }
                    if (_current.TokenType != closeType || _next.TokenType == closeType)
                    {
                        if (_next.TokenType == closeType)
                            Take();

                        while (_current.TokenType != closeType && _current.TokenType != TokenType.EOF)
                            Take();
                    }

                    _error = false;
                }
                if (closeType == TokenType.Semicolon)
                {
                    TakeSemicolon();
                }
                else
                {
                    Take(closeType);
                }
            }
        }

        #endregion

        #region Parse Statements

        private ImportStatement ParseImportStatement()
        {
            var start = Take("import");

            List<IdentifierExpression> moduleNameParts = new List<IdentifierExpression>();

            while (_current.TokenType == TokenType.Identifier && _current.TokenType != TokenType.Semicolon)
            {
                moduleNameParts.Add(new IdentifierExpression(CreateSpan(_current.SourceFilePart.Start), ParseName()));

                if (_current.TokenType != TokenType.Dot && _current.TokenType != TokenType.Semicolon)
                    throw UnexpectedToken("'.' or ';'");

                if (_current.TokenType == TokenType.Dot)
                    Advance();
            }

            Take(TokenType.Semicolon);

            return new ImportStatement(CreateSpan(start.SourceFilePart.Start), moduleNameParts);
        }
        private SyntaxNode ParseStatement()
        {
            SyntaxNode value = null;

            if (_current.TokenType == TokenType.Keyword)
            {
                switch (_current.Value)
                {
                    case "true":
                    case "false":
                        value = ParseExpression();
                        break;

                    case "if":
                        value = ParseIfStatement();
                        break;

                    case "do":
                        value = ParseDoWhileStatement();
                        break;

                    case "while":
                        value = ParseWhileStatement();
                        break;

                    case "for":
                        value = ParseForStatement();
                        break;

                    case "switch":
                        value = ParseSwitchStatement();
                        break;

                    case "return":
                        value = ParseReturnStatement();
                        break;

                    case "var":
                        value = ParseVariableDeclaration();
                        break;

                    default:
                        throw UnexpectedToken("if, do, while, or switch");
                }
            }
            else if (_current.TokenType == TokenType.Semicolon)
            {
                var token = TakeSemicolon();
                AddError(Severity.Warning, "Possibly mistaken empty statement", token.SourceFilePart);

                return new EmptyStatement(token.SourceFilePart);
            }
            else
            {
                MakeStatement(() =>
                {
                    value = ParseExpression();
                });
                return value;
            }
            if (_last.TokenType != TokenType.RightBracket)
            {
                TakeSemicolon();
            }
            return value;
        }
        private CaseStatement ParseCaseStatement()
        {
            List<Expression> conditions = new List<Expression>();
            List<SyntaxNode> contents = new List<SyntaxNode>();
            var start = _current;

            while (_current.Value == "case" || _current.Value == "default")
            {
                if (_current.Value == "default")
                {
                    var word = Take();
                    Take(TokenType.Colon);
                    var span = CreateSpan(word);
                    var condition = new BinaryExpression(span,
                                                             new ConstantExpression(span, "true", ConstantKind.Boolean),
                                                             new ConstantExpression(span, "true", ConstantKind.Boolean),
                                                         BinaryOperator.Equal);
                    conditions.Add(condition);
                }
                else
                {
                    Take();
                    var condition = ParseExpression();
                    Take(TokenType.Colon);
                    conditions.Add(condition);
                }
            }

            while (_current.Value != "case" && _current.TokenType != TokenType.RightBracket)
            {
                contents.Add(ParseStatement());
            }

            return new CaseStatement(CreateSpan(start), conditions, contents);
        }
        private WhileStatement ParseDoWhileStatement()
        {
            var start = TakeKeyword("do");
            var body = ParseStatementOrScope();

            TakeKeyword("while");
            var predicate = ParsePredicate();

            return new WhileStatement(CreateSpan(start), true, predicate, body);
        }
        private ElseStatement ParseElseStatement()
        {
            var start = TakeKeyword("else");

            var body = ParseStatementOrScope();
            return new ElseStatement(CreateSpan(start), body);
        }
        private BlockStatement ParseExpressionOrScope()
        {
            if (_current.TokenType == TokenType.LeftBracket)
            {
                var result = ParseScope();

                return result;
            }
            else
            {
                var start = _current;

                return new BlockStatement(CreateSpan(start), new[] { ParseExpression() });
            }
        }
        private ForStatement ParseForStatement()
        {
            var start = TakeKeyword("for");
            SyntaxNode initialization = null;
            Expression condition = null;
            Expression increment = null;
            MakeBlock(() =>
            {
                if (_current.Value == "var")
                {
                    initialization = ParseVariableDeclaration();
                }
                else if (_current.TokenType == TokenType.Semicolon)
                {
                    initialization = new EmptyStatement(_current.SourceFilePart);
                }
                else
                {
                    initialization = ParseExpression();
                }
                TakeSemicolon();

                if (_current.TokenType == TokenType.Semicolon)
                {
                    condition = new BinaryExpression(
                        _current.SourceFilePart, 
                        new ConstantExpression(_current.SourceFilePart, "1", ConstantKind.Integer), 
                        new ConstantExpression(_current.SourceFilePart, "1", ConstantKind.Integer),
                        BinaryOperator.Equal);
                }
                else
                {
                    condition = ParseLogicalExpression();
                }

                TakeSemicolon();

                if (_current.TokenType == TokenType.RightParenthesis)
                {

                }
                else
                {
                    increment = ParseExpression();
                }

                
            }, TokenType.LeftParenthesis, TokenType.RightParenthesis);

            var block = ParseStatementOrScope();

            return new ForStatement(CreateSpan(start), initialization, condition, increment, block);
        }
        private IfStatement ParseIfStatement()
        {
            var start = TakeKeyword("if");
            var predicate = ParsePredicate();
            var body = ParseStatementOrScope();

            ElseStatement elseStatement = null;
            if (_current.Value == "else")
            {
                elseStatement = ParseElseStatement();
            }

            return new IfStatement(CreateSpan(start), predicate, body, elseStatement);
        }
        private ReturnStatement ParseReturnStatement(bool takeKeyword = true)
        {
            IToken start = null;

            if (takeKeyword)
                start = TakeKeyword("return");
            else
                start = _current;

            var value = ParseExpression();

            return new ReturnStatement(CreateSpan(start), value);
        }
        private BlockStatement ParseScope()
        {
            var contents = new List<SyntaxNode>();
            var start = _current;
            MakeBlock(() =>
            {
                contents.Add(ParseStatement());
            });

            return new BlockStatement(CreateSpan(start), contents);
        }
        private BlockStatement ParseStatementOrScope()
        {
            if (_current.TokenType == TokenType.LeftBracket)
            {
                return ParseScope();
            }
            else
            {
                var statement = ParseStatement();
                return new BlockStatement(statement.FilePart, new[] { statement });
            }
        }
        private SwitchStatement ParseSwitchStatement()
        {
            var cases = new List<CaseStatement>();
            var start = TakeKeyword("switch");

            Expression expr = null;
            MakeBlock(() => expr = ParseExpression(), TokenType.LeftParenthesis, TokenType.RightParenthesis);
            MakeBlock(() =>
            {
                while (_current.Value == "case" || _current.Value == "default")
                {
                    cases.Add(ParseCaseStatement());
                }
            });

            return new SwitchStatement(CreateSpan(start), expr, cases);
        }
        private WhileStatement ParseWhileStatement()
        {
            var start = TakeKeyword("while");
            var expr = ParsePredicate();
            var body = ParseStatementOrScope();

            return new WhileStatement(CreateSpan(start), false, expr, body);
        }

        #endregion

        #region Parse Expressions

        private bool IsAdditiveOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsAssignmentOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.Assignment:
                case TokenType.PlusEqual:
                case TokenType.MinusEqual:
                case TokenType.MulEqual:
                case TokenType.DivEqual:
                case TokenType.ModEqual:
                case TokenType.BitwiseAndEqual:
                case TokenType.BitwiseOrEqual:
                case TokenType.BitwiseXorEqual:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsBitwiseOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.BitwiseAnd:
                case TokenType.BitwiseOr:
                case TokenType.BitwiseXor:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsEqualityOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.Equal:
                case TokenType.NotEqual:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsLogicalOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.BooleanOr:
                case TokenType.BooleanAnd:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsMultiplicativeOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.Mul:
                case TokenType.Div:
                case TokenType.Mod:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsPrefixUnaryOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.PlusPlus:
                case TokenType.MinusMinus:
                case TokenType.Not:
                case TokenType.Minus:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsRelationalOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.GreaterThan:
                case TokenType.LessThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.LessThanOrEqual:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsShiftOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.BitShiftLeft:
                case TokenType.BitShiftRight:
                    return true;

                default:
                    return false;
            }
        }
        private bool IsSuffixUnaryOperator()
        {
            switch (_current.TokenType)
            {
                case TokenType.PlusPlus:
                case TokenType.MinusMinus:
                    return true;

                default:
                    return false;
            }
        }

        private Expression ParseAdditiveExpression()
        {
            Expression left = ParseMultiplicativeExpression();
            while (IsAdditiveOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseMultiplicativeExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private Expression ParseArrayAccessExpression(Expression hint)
        {
            List<Expression> arguments = new List<Expression>();

            MakeBlock(() =>
            {
                arguments.Add(ParseExpression());
                while (_current.TokenType == TokenType.Comma)
                {
                    Take(TokenType.Comma);
                    arguments.Add(ParseExpression());
                }
            }, TokenType.LeftBrace, TokenType.RightBrace);

            var expr = new ArrayAccessExpression(CreateSpan(hint), hint, arguments);

            if (_current.TokenType == TokenType.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }
        private Expression ParseAssignmentExpression()
        {
            Expression left = ParseLogicalExpression();
            if (IsAssignmentOperator())
            {   // Assignment is right-associative.
                var op = ParseBinaryOperator();
                Expression right = ParseAssignmentExpression();

                return new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private BinaryOperator ParseBinaryOperator()
        {
            var token = Take();
            switch (token.TokenType)
            {
                case TokenType.Plus: return BinaryOperator.Add;
                case TokenType.Minus: return BinaryOperator.Sub;
                case TokenType.Assignment: return BinaryOperator.Assign;
                case TokenType.PlusEqual: return BinaryOperator.AddAssign;
                case TokenType.MinusEqual: return BinaryOperator.SubAssign;
                case TokenType.MulEqual: return BinaryOperator.MulAssign;
                case TokenType.DivEqual: return BinaryOperator.DivAssign;
                case TokenType.ModEqual: return BinaryOperator.ModAssign;
                case TokenType.BitwiseAndEqual: return BinaryOperator.AndAssign;
                case TokenType.BitwiseOrEqual: return BinaryOperator.OrAssign;
                case TokenType.BitwiseXorEqual: return BinaryOperator.XorAssign;
                case TokenType.BitwiseAnd: return BinaryOperator.BitwiseAnd;
                case TokenType.BitwiseOr: return BinaryOperator.BitwiseOr;
                case TokenType.BitwiseXor: return BinaryOperator.BitwiseXor;
                case TokenType.Equal: return BinaryOperator.Equal;
                case TokenType.NotEqual: return BinaryOperator.NotEqual;
                case TokenType.BooleanOr: return BinaryOperator.LogicalOr;
                case TokenType.BooleanAnd: return BinaryOperator.LogicalAnd;
                case TokenType.Mul: return BinaryOperator.Mul;
                case TokenType.Div: return BinaryOperator.Div;
                case TokenType.Mod: return BinaryOperator.Div;
                case TokenType.GreaterThan: return BinaryOperator.GreaterThan;
                case TokenType.LessThan: return BinaryOperator.LessThan;
                case TokenType.GreaterThanOrEqual: return BinaryOperator.GreaterThanOrEqual;
                case TokenType.LessThanOrEqual: return BinaryOperator.LessThanOrEqual;
                case TokenType.BitShiftLeft: return BinaryOperator.LeftShift;
                case TokenType.BitShiftRight: return BinaryOperator.RightShift;
            }

            _index--;
            throw UnexpectedToken("Binary Operator");
        }
        private Expression ParseBitwiseExpression()
        {
            Expression left = ParseShiftExpression();

            while (IsBitwiseOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseShiftExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private Expression ParseConstantExpression()
        {
            ConstantKind kind;

            if (_current.Value == "true" || _current.Value == "false")
            {
                kind = ConstantKind.Boolean;
            }
            else if (_current.TokenType == TokenType.StringLiteral)
            {
                kind = ConstantKind.String;
            }
            else if (_current.TokenType == TokenType.IntegerLiteral)
            {
                kind = ConstantKind.Integer;
            }
            else if (_current.TokenType == TokenType.RealLiteral)
            {
                kind = ConstantKind.Real;
            }
            else
            {
                throw UnexpectedToken("Constant");
            }

            var token = Take();

            var expr = new ConstantExpression(token.SourceFilePart, token.Value, kind);

            if (_current.TokenType == TokenType.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }
        private Expression ParseEqualityExpression()
        {
            Expression left = ParseRelationalExpression();
            while (IsEqualityOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseRelationalExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private Expression ParseExpression()
        {
            return ParseAssignmentExpression();
        }
        private Expression ParseIdentiferExpression()
        {
            var token = Take(TokenType.Identifier);

            return new IdentifierExpression(token.SourceFilePart, token.Value);
        }
        private LambdaExpression ParseLambdaExpression(ISourceFileLocation start, IEnumerable<ParameterDeclaration> arguments)
        {
            Take(TokenType.FatArrow);

            var body = ParseExpressionOrScope();

            if (_current.TokenType != TokenType.Semicolon)
                throw UnexpectedToken(TokenType.Semicolon);

            return new LambdaExpression(CreateSpan(start), arguments, body);
        }
        private Expression ParseLogicalExpression()
        {
            Expression left = ParseEqualityExpression();
            while (IsLogicalOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseEqualityExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private Expression ParseMethodCallExpression()
        {
            var hint = ParseIdentiferExpression();
            return ParseMethodCallExpression(hint);
        }
        private Expression ParseMethodCallExpression(Expression reference)
        {
            var arguments = new List<Expression>();
            MakeBlock(() =>
            {
                if (_current.TokenType != TokenType.RightParenthesis)
                {
                    arguments.Add(ParseExpression());
                    while (_current.TokenType == TokenType.Comma)
                    {
                        Take(TokenType.Comma);
                        arguments.Add(ParseExpression());
                    }
                }
            }, TokenType.LeftParenthesis, TokenType.RightParenthesis);

            var expr = new MethodCallExpression(CreateSpan(reference), reference, arguments);
            if (_current.TokenType == TokenType.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }
        private Expression ParseMultiplicativeExpression()
        {
            Expression left = ParseUnaryExpression();
            while (IsMultiplicativeOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseUnaryExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private Expression ParseNewExpression()
        {
            var start = TakeKeyword("new");
            List<Expression> references = new List<Expression>();
            List<Expression> arguments = new List<Expression>();

            Expression reference;

            references.Add(ParseIdentiferExpression());
            while (_current.TokenType == TokenType.Dot)
            {
                Take(TokenType.Dot);
                references.Add(ParseIdentiferExpression());
            }

            if (references.Count == 1)
            {
                reference = references.First();
            }
            else
            {
                reference = new ReferenceExpression(CreateSpan(references.First()), references);
            }

            MakeBlock(() =>
            {
                if (_current.TokenType != TokenType.RightParenthesis)
                {
                    arguments.Add(ParseExpression());
                    while (_current.TokenType == TokenType.Comma)
                    {
                        arguments.Add(ParseExpression());
                    }
                }
            }, TokenType.LeftParenthesis, TokenType.RightParenthesis);

            var expr = new NewExpression(CreateSpan(start), reference, arguments);
            if (_current.TokenType == TokenType.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }
        private Expression ParseOverrideExpression()
        {
            var start = Take(TokenType.LeftParenthesis).SourceFilePart.Start;

            if (_current.TokenType == TokenType.RightParenthesis)
            {
                Take(TokenType.RightParenthesis);
                return ParseLambdaExpression(start, new ParameterDeclaration[0]);
            }
            if (_current.TokenType == TokenType.Identifier
            && (_next.TokenType == TokenType.Comma
                || (_next.TokenType == TokenType.RightParenthesis && Peek(2).TokenType == TokenType.FatArrow)
                || _next.TokenType == TokenType.Colon))
            {
                _index--;
                var parameters = ParseParameterList();
                return ParseLambdaExpression(start, parameters);
            }
            var expr = ParseExpression();

            Take(TokenType.RightParenthesis);

            if (_current.TokenType == TokenType.LeftParenthesis)
            {
                return ParseMethodCallExpression(expr);
            }
            else if (_current.TokenType == TokenType.Dot)
            {
                return ParseReferenceExpression(expr);
            }

            return expr;
        }
        private Expression ParsePredicate()
        {
            Expression expr = null;

            MakeBlock(() =>
            {
                expr = ParseLogicalExpression();
            }, TokenType.LeftParenthesis, TokenType.RightParenthesis);

            return expr;
        }
        private UnaryOperator ParsePrefixUnaryOperator()
        {
            var token = _current;
            Advance();
            switch (token.TokenType)
            {
                case TokenType.PlusPlus: return UnaryOperator.PreIncrement;
                case TokenType.MinusMinus: return UnaryOperator.PreDecrement;
                case TokenType.Not: return UnaryOperator.Not;
                case TokenType.Minus: return UnaryOperator.Negation;
            }
            _index--;
            throw UnexpectedToken("Unary Operator");
        }
        private Expression ParsePrimaryExpression()
        {
            if (_current.TokenType == TokenType.Identifier)
            {
                if (_next.TokenType == TokenType.Dot)
                {
                    return ParseReferenceExpression();
                }
                else if (_next.TokenType == TokenType.LeftParenthesis)
                {
                    return ParseMethodCallExpression();
                }
                else if (_next.TokenType == TokenType.LeftBrace)
                {
                    return ParseArrayAccessExpression(ParseIdentiferExpression());
                }
                return ParseIdentiferExpression();
            }
            else if (_current.Category == TokenCategory.Constant || _current.Value == "true" || _current.Value == "false")
            {
                return ParseConstantExpression();
            }
            else if (_current.TokenType == TokenType.LeftParenthesis)
            {
                return ParseOverrideExpression();
            }
            else if (_current.Value == "new")
            {
                return ParseNewExpression();
            }
            else if (_current.TokenType == TokenType.Semicolon && _last.Value == "return")
            {
                return null;
            }
            else
            {
                if (_current.Category == TokenCategory.Operator)
                {
                    var token = _current;

                    Advance();

                    throw SyntaxError(Severity.Error, $"'{token.Value}' is an invalid expression term", token.SourceFilePart);
                }

                throw UnexpectedToken("Expression Term");
            }
        }
        private Expression ParseReferenceExpression(Expression hint)
        {
            var references = new List<Expression>();
            references.Add(hint);

            do
            {
                Take(TokenType.Dot);
                if (_current.TokenType == TokenType.Identifier)
                {
                    var expr = ParseIdentiferExpression();
                    references.Add(expr);
                }

                if (_current.TokenType == TokenType.LeftParenthesis)
                {
                    var expr = new ReferenceExpression(CreateSpan(hint), references);
                    return ParseMethodCallExpression(expr);
                }
                else if (_current.TokenType == TokenType.LeftBrace)
                {
                    var expr = new ReferenceExpression(CreateSpan(hint), references);
                    return ParseArrayAccessExpression(expr);
                }
            } while (_current.TokenType == TokenType.Dot);

            return new ReferenceExpression(CreateSpan(hint), references);
        }
        private Expression ParseReferenceExpression()
        {
            var hint = ParseIdentiferExpression();
            return ParseReferenceExpression(hint);
        }
        private Expression ParseRelationalExpression()
        {
            Expression left = ParseBitwiseExpression();

            while (IsRelationalOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseBitwiseExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private Expression ParseShiftExpression()
        {
            Expression left = ParseAdditiveExpression();
            while (IsShiftOperator())
            {
                var op = ParseBinaryOperator();
                var right = ParseAdditiveExpression();
                left = new BinaryExpression(CreateSpan(left), left, right, op);
            }
            return left;
        }
        private UnaryOperator ParseSuffixUnaryOperator()
        {
            var token = _current;
            Advance();
            switch (token.TokenType)
            {
                case TokenType.PlusPlus: return UnaryOperator.PostIncrement;
                case TokenType.MinusMinus: return UnaryOperator.PostDecrement;
            }
            _index--;
            throw UnexpectedToken("Unary Operator");
        }
        private Expression ParseUnaryExpression()
        {
            UnaryOperator op = UnaryOperator.Default;
            ISourceFileLocation start = null;

            if (IsPrefixUnaryOperator())
            {
                start = _current.SourceFilePart.Start;

                op = ParsePrefixUnaryOperator();
            }

            if (start == null)
            {
                start = _current.SourceFilePart.Start;
            }
            var expression = ParsePrimaryExpression();

            if (IsSuffixUnaryOperator())
            {
                op = ParseSuffixUnaryOperator();
            }

            if (op != UnaryOperator.Default)
            {
                return new UnaryExpression(CreateSpan(start), expression, op);
            }
            return expression;
        }

        #endregion

        #region Parse Declarations

        private string ParseName()
        {
            if (_current.TokenType == TokenType.Identifier)
                return Take(TokenType.Identifier).Value;

            if (_current.IsTypeKeyword())
                return Take(TokenType.Keyword).Value;

            throw UnexpectedToken("Type keyword or identifier");
        }
        private DeclarationVisibility? ParseVisibility(bool optional = false)
        {
            switch (_current.Value)
            {
                case "private":
                    Take();
                    return DeclarationVisibility.Private;
                case "protected":
                    Take();
                    return DeclarationVisibility.Protected;
                case "public":
                    Take();
                    return DeclarationVisibility.Public;
            }

            if (!optional)
                throw UnexpectedToken("Visibility modifier, 'public', 'protected', 'private'");

            return null;
        }
        private ModuleDeclaration ParseModuleDeclaration()
        {
            var start = Take("module");

            var moduleNameParts = new List<IdentifierExpression>();
            var classes = new List<ClassDeclaration>();
            var methods = new List<MethodDeclaration>();

            while (_current.TokenType == TokenType.Identifier)
            {
                moduleNameParts.Add(new IdentifierExpression(CreateSpan(_current.SourceFilePart.Start), ParseName()));

                if (_current.TokenType == TokenType.Dot)
                    Advance();
            }

            MakeBlock(() =>
            {
                var visibility = ParseVisibility(optional: false);

                // TODO(Dan): Allow stand-alone functions in a module!

                while (_current.Value == "class" || _current.IsTypeKeyword() || _current.TokenType == TokenType.Identifier)
                {

                    if (_current.Value == "class")
                    {
                        classes.Add(ParseClassDeclaration(visibility.Value));
                    }
                    else
                    {
                        var returnType = ParseTypeDeclaration();
                        var name = ParseName();
                        methods.Add(ParseMethodDeclaration(visibility.Value, name, returnType));
                    }

                }
            });

            return new ModuleDeclaration(CreateSpan(start.SourceFilePart.Start), string.Join(".", moduleNameParts.Select(identifier => identifier.Identifier)), classes, methods);
        }
        private ClassDeclaration ParseClassDeclaration(DeclarationVisibility visibility)
        {
            var ctors = new List<ConstructorDeclaration>();
            var props = new List<PropertyDeclaration>();
            var methods = new List<MethodDeclaration>();
            var fields = new List<FieldDeclaration>();
            var typeParameters = new List<TypeDeclaration>();

            var start = TakeKeyword("class");
            var name = ParseName();

            if (_current.TokenType == TokenType.LessThan)
                typeParameters.AddRange(ParseGenericTypes());

            MakeBlock(() =>
            {
                var member = ParseClassMember();
                switch (member)
                {
                    case PropertyDeclaration property:
                        props.Add(property);
                        break;

                    case FieldDeclaration field:
                        fields.Add(field);
                        break;

                    case MethodDeclaration method:
                        methods.Add(method);
                        break;

                    case ConstructorDeclaration constructor:
                        ctors.Add(constructor);
                        break;
                }
            });

            return new ClassDeclaration(CreateSpan(start), name, visibility, ctors, fields, methods, props, typeParameters);
        }
        private SyntaxNode ParseClassMember()
        {
            var visibility = ParseVisibility();

            if (_current.Value == "constructor")
            {
                return ParseConstructorDeclaration(visibility.Value);
            }

            var returnType = ParseTypeDeclaration();
            var name = ParseName();

            switch (_current.Value)
            {
                case "{":
                case "=>":
                    return ParsePropertyDeclaration(visibility.Value, name, returnType);

                case "(":
                case "<":
                    return ParseMethodDeclaration(visibility.Value, name, returnType);
                   
                case ";":
                case "=":
                    return ParseFieldDeclaration(visibility.Value, name, returnType);

                default:
                    throw UnexpectedToken("Field, Property or Method Declaration: '{', '=>', '(', '<', ';', '='");
            }
        }
        private ConstructorDeclaration ParseConstructorDeclaration(DeclarationVisibility visibility)
        {
            var start = TakeKeyword("constructor");
            var parameters = ParseParameterList();
            var body = ParseScope();

            return new ConstructorDeclaration(CreateSpan(start), visibility, parameters, body);
        }
        private SourceDocument ParseDocument()
        {
            var imports = new List<ImportStatement>();
            var modules = new List<ModuleDeclaration>();

            var start = _current.SourceFilePart.Start;

            while (_current.Value == "import")
            {
                imports.Add(ParseImportStatement());
            }

            while (_current.Value == "module")
            {
                modules.Add(ParseModuleDeclaration());
            }

            if (_current.TokenType != TokenType.EOF)
            {
                // TODO(Dan): Need to have a better error message here...
                AddError(Severity.Error, "Top-level statements are not permitted. Statements must be part of a module except for import statements which are at the top of the file", CreateSpan(_current.SourceFilePart.Start, _tokens.Last().SourceFilePart.End));
            }

            return new SourceDocument(CreateSpan(start), _sourceFile, imports, modules);
        }
        private FieldDeclaration ParseFieldDeclaration(DeclarationVisibility visibility, string name, TypeDeclaration type)
        {
            var start = _current;

            Expression defaultValue = null;
            if (_current.TokenType == TokenType.Assignment)
            {
                Take();
                defaultValue = ParseExpression();
            }

            Take(TokenType.Semicolon);

            return new FieldDeclaration(CreateSpan(start), name, visibility, type, defaultValue);
        }
        private BlockStatement ParseLambdaOrScope()
        {
            if (_current.TokenType == TokenType.FatArrow)
            {
                Take();
                var expr = ParseExpression();
                //TakeSemicolon();
                return new BlockStatement(expr.FilePart, new[] { expr });
            }
            else
            {
                return ParseScope();
            }
        }
        private MethodDeclaration ParseMethodDeclaration(DeclarationVisibility visibility, string name, TypeDeclaration returnType)
        {
            var start = _current;
            //var returnType = ParseTypeDeclaration();
            //var name = ParseName();
            var genericTypes = Enumerable.Empty<TypeDeclaration>();

            if (_current.TokenType == TokenType.LessThan)
                genericTypes = ParseGenericTypes().ToList();

            var parameters = ParseParameterList();

            if (_current.TokenType == TokenType.FatArrow)
            {
                var method = ParseExpressionBodiedMember(name, visibility, returnType, parameters);

                TakeSemicolon();

                return method;
            }
            
            var body = ParseLambdaOrScope();

            return new MethodDeclaration(CreateSpan(start), name, visibility, returnType, genericTypes, parameters, body);
        }
        private ParameterDeclaration ParseParameterDeclaration()
        {
            var type = ParseTypeDeclaration();
            var name = Take(TokenType.Identifier);

            if (_current.TokenType == TokenType.Colon)
            {
                type = ParseTypeDeclaration();
                Take();
            }

            return new ParameterDeclaration(CreateSpan(name), name.Value, type);
        }
        private IEnumerable<ParameterDeclaration> ParseParameterList()
        {
            List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();
            MakeBlock(() =>
            {
                if (_current.TokenType == TokenType.RightParenthesis)
                {
                    return;
                }
                parameters.Add(ParseParameterDeclaration());
                while (_current.TokenType == TokenType.Comma)
                {
                    Take(TokenType.Comma);
                    parameters.Add(ParseParameterDeclaration());
                }
            }, TokenType.LeftParenthesis, TokenType.RightParenthesis);

            return parameters;
        }
        private PropertyDeclaration ParsePropertyDeclaration(DeclarationVisibility visibility, string name, TypeDeclaration type)
        {
            var start = _current;
            //var type = ParseTypeDeclaration();
            //var name = ParseName();

            MethodDeclaration getMethod = null;
            MethodDeclaration setMethod = null;

            if (_current.TokenType == TokenType.FatArrow)
            {
                // TODO(Dan): Implement expression bodied members
                //throw new NotImplementedException("Expression bodied members are currently not implemented");
                getMethod = ParseExpressionBodiedMember(methodName: $"get_{name}", visibility: visibility, returnType: type, parameters: new ParameterDeclaration[0]);
                

                TakeSemicolon();
            }
            else
            {
                MakeBlock(() =>
                {
                    var setVisibility = ParseVisibility(optional: true);

                    switch (_current.Value)
                    {
                        case "get":
                            {
                                var getStart = Take();

                                switch(_current.TokenType)
                                {
                                    case TokenType.Semicolon:
                                        throw new NotImplementedException("Bodyless property getter not yet supported");

                                    // TODO(Dan): Expression bodied members
                                    case TokenType.FatArrow:
                                        getMethod = ParseExpressionBodiedMember(methodName: $"get_{name}", visibility: visibility, returnType: type, parameters: new ParameterDeclaration[0]);
                                        TakeSemicolon();
                                        break;

                                    default:
                                        {
                                            var body = ParseLambdaOrScope();
                                            if (getMethod != null)
                                            {
                                                AddError(Severity.Error, "Multiple getters", CreateSpan(getStart));
                                            }
                                            else
                                            {
                                                getMethod = new MethodDeclaration(CreateSpan(getStart), $"get_{name}", visibility, type, new ParameterDeclaration[0], body);
                                            }
                                        }
                                        break;
                                }

                                break;
                            }
                        case "set":
                            {
                                var setStart = Take();

                                switch (_current.TokenType)
                                {
                                    case TokenType.Semicolon:
                                        throw new NotImplementedException("Bodyless property setter not yet supported");

                                    // TODO(Dan): Expression bodied members
                                    case TokenType.FatArrow:
                                        setMethod = ParseExpressionBodiedMember(methodName: $"set_{name}", visibility: setVisibility ?? visibility, returnType: new TypeDeclaration(null, "void"), parameters: new[] { new ParameterDeclaration(setStart.SourceFilePart, "value", type) });
                                        TakeSemicolon();
                                        break;

                                    default:
                                        {
                                            var body = ParseLambdaOrScope();
                                            if (setMethod != null)
                                            {
                                                AddError(Severity.Error, "Multiple setters", CreateSpan(setStart));
                                            }
                                            else
                                            {
                                                setMethod = new MethodDeclaration(CreateSpan(setStart), $"set_{name}", setVisibility ?? visibility, new TypeDeclaration(null, "void"),
                                                                          new[] { new ParameterDeclaration(setStart.SourceFilePart, "value", type) }, body);
                                            }
                                        }
                                        break;
                                }

                                break;
                            }
                        default:
                            throw UnexpectedToken("get or set");
                    }


                });
            }

            if (_current.TokenType == TokenType.Semicolon)
            {
                var token = TakeSemicolon();
                AddError(Severity.Warning, "Possibly mistaken empty statement", token.SourceFilePart);
            }

            // TODO(Dan): Do we want to enable auto properties?
            if (getMethod == null)
            {
                AddError(Severity.Error, $"Property \"{name}\" does not have a getter", CreateSpan(start));
            }

            return new PropertyDeclaration(CreateSpan(start), name, visibility, type, getMethod, setMethod);
        }
        private TypeDeclaration ParseTypeDeclaration()
        {
            var name = ParseName();

            switch (_current.TokenType)
            {
                case TokenType.Dot:
                    {
                        while (_current.TokenType == TokenType.Dot)
                        {
                            Advance();
                            name += ".";

                            name += ParseName();
                        }
                    }
                    break;

                case TokenType.LeftBrace:
                    {
                        if (_next.TokenType == TokenType.RightBrace)
                        {
                            name += "[]";

                            Advance();
                            Advance();
                        }
                        else
                        {
                            throw UnexpectedToken("']'");
                        }
                    }
                    break;

                case TokenType.LessThan:
                    {
                        name += "<";

                        var genericTypes = ParseGenericTypes().Select(t => t.Name);

                        name += string.Join(", ", genericTypes);

                        name += ">";
                    }
                    break;

                //default:
                //    throw UnexpectedToken("'.', '[]', '<' or '>'");
            }
            
            return new TypeDeclaration(_current.SourceFilePart, name);
        }
        private VariableDeclaration ParseVariableDeclaration()
        {
            var start = TakeKeyword("var");
            var name = ParseName();
            var type = new TypeDeclaration(null, "object");
            Expression value = null;

            // NOTE(Dan): If we want var name : string = "";
            //if (_current.TokenType == TokenType.Colon)
            //{
            //    Take();
            //    type = ParseTypeDeclaration();
            //}
            if (_current.TokenType == TokenType.Assignment)
            {
                Take();

                value = ParseExpression();
            }

            return new VariableDeclaration(CreateSpan(start), name, type, value);
        }
        private IEnumerable<TypeDeclaration> ParseGenericTypes()
        {
            Take(TokenType.LessThan);

            yield return new TypeDeclaration(CreateSpan(_current.SourceFilePart.Start), ParseName());

            while (_current.TokenType == TokenType.Comma)
            {
                Take(TokenType.Comma);

                yield return new TypeDeclaration(CreateSpan(_current.SourceFilePart.Start), ParseName());
            }

            Take(TokenType.GreaterThan);
        }
        private MethodDeclaration ParseExpressionBodiedMember(string methodName, DeclarationVisibility visibility, TypeDeclaration returnType, IEnumerable<ParameterDeclaration> parameters)
        {
            Take(TokenType.FatArrow);

            var expression = ParseExpression();
            var span = CreateSpan(expression.FilePart.Start, expression.FilePart.End);

            ReturnStatement returnStatement = null;

            if (returnType.Name != "void")
                returnStatement = new ReturnStatement(span, expression);

            return new MethodDeclaration(span, methodName, visibility, returnType, parameters, new BlockStatement(span, new[] { (SyntaxNode)returnStatement ?? expression }));
        }
        
        #endregion

        #region Create Span

        private ISourceFilePart CreateSpan(ISourceFileLocation start, ISourceFileLocation end)
        {
            var content = _sourceFile.Lines.Skip(start.Line - 1).Take(end.Line - 1).ToArray();
            return new SourceFilePart(_sourceFile.Name, content, start, end);
        }
        private ISourceFilePart CreateSpan(IToken start)
        {
            return CreateSpan(start.SourceFilePart.Start, _current.SourceFilePart.End);
        }
        private ISourceFilePart CreateSpan(SyntaxNode start)
        {
            return CreateSpan(start.FilePart.Start, _current.SourceFilePart.End);
        }
        private ISourceFilePart CreateSpan(ISourceFileLocation start)
        {
            return CreateSpan(start, _current.SourceFilePart.End);
        }

        #endregion

        #region Error

        private void AddError(Severity severity, string message, ISourceFilePart span = null)
        {
            _errorSink.AddError($"{message} in '{_sourceFile.Name}'", span, severity);
        }
        private SyntaxException UnexpectedToken(TokenType expected)
        {
            return UnexpectedToken($"'{expected.GetValue()}'");
        }
        private SyntaxException UnexpectedToken(string expected)
        {
            Advance();

            var value = string.IsNullOrEmpty(_last?.Value)
                ? _last?.TokenType.ToString()
                : _last?.Value;

            var message = $"Unexpected '{value}'. Expected {expected}";

            return SyntaxError(Severity.Error, message, _last.SourceFilePart);
        }
        private SyntaxException SyntaxError(Severity severity, string message, ISourceFilePart span = null)
        {
            _error = true;
            AddError(severity, message, span);
            return new SyntaxException(message);
        }

        #endregion

        public SyntaxParser() : this(new Tokenizer(TokenizerGrammar.Default))
        {

        }
        public SyntaxParser(ITokenizer tokenizer) : this(options => { }, tokenizer, tokenizer.ErrorSink)
        {
        }
        public SyntaxParser(Action<ParserOptions> options, ITokenizer tokenizer, IErrorSink errorSink)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (tokenizer == null)
                throw new ArgumentNullException(nameof(tokenizer));

            if (errorSink == null)
                throw new ArgumentNullException(nameof(errorSink));

            _tokenizer = tokenizer;
            _errorSink = errorSink;
            _options = new ParserOptions();

            options(_options);
        }
    }
}
