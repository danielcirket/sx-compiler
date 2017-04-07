using System;
using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;
using Sx.Lexer.Abstractions;
using Sxc.Compiler.Parser.Abstractions;

namespace Sx.Compiler.Parser
{
    public class Parser
    {
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

        public SyntaxNode Parse(ISourceFile sourceFile, IEnumerable<IToken> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            _sourceFile = sourceFile;
            _tokens = tokens.Where(t => !t.IsTrivia());
            _index = 0;

            try
            {
                return ParseInternal();
            }
            catch (SyntaxException ex)
            {
                return null;
            }
        }

        private SyntaxNode ParseInternal()
        {
            return ParseDocument();
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
            if (_current.TokenType == TokenType.Semicolon)
            {
                return Take(TokenType.Semicolon);
            }

            return _current;
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
                while (_current.TokenType != closeType && _current.TokenType != TokenType.EOF)
                {
                    action();
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

            while(_current.TokenType == TokenType.Identifier && _current.TokenType != TokenType.Semicolon)
            {
                moduleNameParts.Add(new IdentifierExpression(CreateSpan(_current.SourceFilePart.Start), ParseName()));

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
                AddError(Severity.Warning, "Possibly mistaken empty statement.", token.SourceFilePart);

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
                return ParseScope();
            }
            else
            {
                var start = _current;
                var expr = ParseExpression();
                return new BlockStatement(CreateSpan(start), new[] { expr });
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

                condition = ParseLogicalExpression();
                TakeSemicolon();

                increment = ParseExpression();
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
        private ReturnStatement ParseReturnStatement()
        {
            var start = TakeKeyword("return");
            var value = ParseExpression();

            return new ReturnStatement(CreateSpan(start), value);
        }
        private BlockStatement ParseScope()
        {
            List<SyntaxNode> contents = new List<SyntaxNode>();
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
            List<CaseStatement> cases = new List<CaseStatement>();

            var start = TakeKeyword("switch");

            Expression expr;
            MakeBlock(() => expr = ParseExpression(), TokenType.LeftParenthesis, TokenType.RightParenthesis);
            MakeBlock(() =>
            {
                while (_current.Value == "case" || _current.Value == "default")
                {
                    cases.Add(ParseCaseStatement());
                }
            });

            return new SwitchStatement(CreateSpan(start), cases);
        }
        private WhileStatement ParseWhileStatement()
        {
            var start = TakeKeyword("while");

            Expression expr = ParsePredicate();

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
                kind = ConstantKind.Float;
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
                return ParseIdentiferExpression();
            }
            else if (_next.TokenType == TokenType.LeftParenthesis)
            {
                return ParseMethodCallExpression();
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
            else
            {
                if (_current.Category == TokenCategory.Operator)
                {
                    var token = _current;

                    Advance();

                    throw SyntaxError(Severity.Error, $"'{token.Value}' is an invalid expression term.", token.SourceFilePart);
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
            return Take(TokenType.Identifier).Value;
        }
        private ModuleDeclaration ParseModuleDeclaration()
        {
            var start = Take("module");

            List<IdentifierExpression> moduleNameParts = new List<IdentifierExpression>();
            List<ClassDeclaration> contents = new List<ClassDeclaration>();

            while (_current.TokenType == TokenType.Identifier)
            {
                moduleNameParts.Add(new IdentifierExpression(CreateSpan(_current.SourceFilePart.Start), ParseName()));

                if (_current.TokenType == TokenType.Dot)
                    Advance();
            }

            MakeBlock(() =>
            {
                while (_current.Value == "class")
                {
                    contents.Add(ParseClassDeclaration());
                }
            });

            return new ModuleDeclaration(CreateSpan(start.SourceFilePart.Start), string.Join(".", moduleNameParts.Select(identifier => identifier.Identifier)), contents);            
        }
        private ClassDeclaration ParseClassDeclaration()
        {
            List<ConstructorDeclaration> ctors = new List<ConstructorDeclaration>();
            List<PropertyDeclaration> props = new List<PropertyDeclaration>();
            List<MethodDeclaration> methods = new List<MethodDeclaration>();
            List<FieldDeclaration> fields = new List<FieldDeclaration>();

            var start = TakeKeyword("class");
            var name = ParseName();

            MakeBlock(() =>
            {
                var member = ParseClassMember();
                switch (member?.Kind)
                {
                    case SyntaxKind.PropertyDeclaration:
                        props.Add(member as PropertyDeclaration);
                        break;

                    case SyntaxKind.FieldDeclaration:
                        fields.Add(member as FieldDeclaration);
                        break;

                    case SyntaxKind.MethodDeclaration:
                        methods.Add(member as MethodDeclaration);
                        break;

                    case SyntaxKind.ConstructorDeclaration:
                        ctors.Add(member as ConstructorDeclaration);
                        break;
                }
            });

            return new ClassDeclaration(CreateSpan(start), name, ctors, fields, methods, props);
        }
        private SyntaxNode ParseClassMember()
        {
            switch (_current.Value)
            {
                case "property":
                    return ParsePropertyDeclaration();

                case "function":
                    return ParseMethodDeclaration();

                case "constructor":
                    return ParseConstructorDeclaration();

                case "field":
                    return ParseFieldDeclaration();

                default:
                    throw UnexpectedToken("property', 'function', 'constructor' or 'field");
            }
        }
        private ConstructorDeclaration ParseConstructorDeclaration()
        {
            var start = TakeKeyword("constructor");
            //var name = ParseName();

            //if (_next.TokenType == TokenType.LeftParenthesis)
            //    UnexpectedToken(TokenType.LeftParenthesis);

            var parameters = ParseParameterList();
            var body = ParseScope();

            return new ConstructorDeclaration(CreateSpan(start), parameters, body);
        }
        private SourceDocument ParseDocument()
        {
            List<SyntaxNode> contents = new List<SyntaxNode>();

            var start = _current.SourceFilePart.Start;

            // TODO(Dan): Deal with "import" statements
            while (_current.Value == "import")
            {
                contents.Add(ParseImportStatement());
            }

            while (_current.Value == "module")
            {
                contents.Add(ParseModuleDeclaration());
            }

            if (_current.TokenType != TokenType.EOF)
            {
                AddError(Severity.Error, "Top-level statements are not permitted.", CreateSpan(_current.SourceFilePart.Start, _tokens.Last().SourceFilePart.End));
            }

            return new SourceDocument(CreateSpan(start), _sourceFile, contents);
        }
        private FieldDeclaration ParseFieldDeclaration()
        {
            var start = _current.SourceFilePart.Start;

            Take("field");

            var type = ParseTypeDeclaration();

            var name = ParseName();
            Expression defaultValue = null;
            if (_current.TokenType == TokenType.Assignment)
            {
                Take();
                defaultValue = ParseExpression();
            }

            Take(TokenType.Semicolon);

            return new FieldDeclaration(CreateSpan(start), name, type, defaultValue);
        }
        private BlockStatement ParseLambdaOrScope()
        {
            if (_current.TokenType == TokenType.FatArrow)
            {
                Take();
                var expr = ParseExpression();
                TakeSemicolon();
                return new BlockStatement(expr.FilePart, new[] { expr });
            }
            else
            {
                return ParseScope();
            }
        }
        private MethodDeclaration ParseMethodDeclaration()
        {
            var start = TakeKeyword("function");
            var returnType = ParseTypeDeclaration();
            var name = ParseName();
            var parameters = ParseParameterList();
            var body = ParseLambdaOrScope();

            return new MethodDeclaration(CreateSpan(start), name, returnType, parameters, body);
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
        private PropertyDeclaration ParsePropertyDeclaration()
        {
            var start = TakeKeyword("property");
            var type = ParseTypeDeclaration();
            var name = ParseName();

            MethodDeclaration getMethod = null;
            MethodDeclaration setMethod = null;

            if (_current.TokenType == TokenType.FatArrow)
            {
                getMethod = ParseLambdaExpression(_current.SourceFilePart.Start, new ParameterDeclaration[0]).ToMethodDeclaration($"get_{name}", type.Name);
                Take(TokenType.Semicolon);
            }
            else
            {
                MakeBlock(() =>
                {
                    switch (_current.Value)
                    {
                        case "get":
                            {
                                var getStart = Take();
                                var body = ParseLambdaOrScope();
                                if (getMethod != null)
                                {
                                    AddError(Severity.Error, "Multiple getters", CreateSpan(getStart));
                                }
                                else
                                {
                                    getMethod = new MethodDeclaration(CreateSpan(getStart), $"get_{name}", type, new ParameterDeclaration[0], body);
                                }
                                break;
                            }
                        case "set":
                            {
                                var setStart = Take();
                                var body = ParseLambdaOrScope();
                                if (setMethod != null)
                                {
                                    AddError(Severity.Error, "Multiple setters", CreateSpan(setStart));
                                }
                                else
                                {
                                    setMethod = new MethodDeclaration(CreateSpan(setStart), $"set_{name}", new TypeDeclaration(null , "void"),
                                                                      new[] { new ParameterDeclaration(setStart.SourceFilePart, "value", type) }, body);
                                }
                                break;
                            }
                        default:
                            throw UnexpectedToken("get or set");
                    }
                });
            }
            if (getMethod == null)
            {
                AddError(Severity.Error, $"Property \"{name}\" does not have a getter!", CreateSpan(start));
            }
            return new PropertyDeclaration(CreateSpan(start), name, type, getMethod, setMethod);
        }
        private TypeDeclaration ParseTypeDeclaration()
        {
            //if (_current.TokenType != TokenType.LessThan)
            //{
            //    throw UnexpectedToken("Type Annotation");
            //}
            //Take(TokenType.LessThan);
            //var identifier = ;
            //Take(TokenType.GreaterThan);

            return new TypeDeclaration(_current.SourceFilePart, ParseName());
        }
        private VariableDeclaration ParseVariableDeclaration()
        {
            var start = TakeKeyword("var");
            var name = ParseName();
            var type = new TypeDeclaration(null, "Object");
            Expression value = null;

            if (_current.TokenType == TokenType.Colon)
            {
                Take();
                type = ParseTypeDeclaration();
            }
            if (_current.TokenType == TokenType.Assignment)
            {
                Take();

                value = ParseExpression();
            }

            return new VariableDeclaration(CreateSpan(start), name, type, value);
        }

        #endregion

        #region Create Span

        private ISourceFilePart CreateSpan(ISourceFileLocation start, ISourceFileLocation end)
        {
            return new SourceFilePart(start, end);
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
            _errorSink.AddError(message, span, severity);
        }
        private SyntaxException UnexpectedToken(TokenType expected)
        {
            return UnexpectedToken(expected.GetValue());
        }
        private SyntaxException UnexpectedToken(string expected)
        {
            Advance();

            var value = string.IsNullOrEmpty(_last?.Value)
                ? _last?.TokenType.ToString()
                : _last?.Value;

            var message = $"Unexpected '{value}'. Expected '{expected}'";

            return SyntaxError(Severity.Error, message, _last.SourceFilePart);
        }
        private SyntaxException SyntaxError(Severity severity, string message, ISourceFilePart span = null)
        {
            _error = true;
            AddError(severity, message, span);
            return new SyntaxException(message);
        }

        #endregion

        public Parser(Action<ParserOptions> options, IErrorSink errorSink)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (errorSink == null)
                throw new ArgumentNullException(nameof(errorSink));

            _errorSink = errorSink;
            _options = new ParserOptions();

            options(_options);
        }
    }
}
