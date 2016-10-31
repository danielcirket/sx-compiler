using System;
using System.Collections.Generic;
using System.Text;
using Sxc.Lexer.Abstractions;

namespace Sxc.Lexer
{
    public class SxcLexer : ILexer
    {
        private readonly string[] _keywords;
        private StringBuilder _builder;
        private int _column;
        private int _index;
        private int _line;
        private ISourceFile _sourceFile;
        private ISourceFileLocation _sourceFileLocation;
        private IErrorSink _errorSink;

        private char _ch => _sourceFile.Contents[_index];
        private char _next => _sourceFile.Contents[_index + 1];
        public IErrorSink ErrorSink => _errorSink;

        public IEnumerable<IToken> Tokenize(ISourceFile sourceFile)
        {
            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            _sourceFile = sourceFile;
            _builder.Clear();
            _line = 1;
            _index = 0;
            _column = 1;
            _sourceFileLocation = new SourceFileLocation(_column, _index, _line);

            return TokenizeInternal();

        }
        public IEnumerable<IToken> Tokenize(string sourceFileContent)
        {
            return Tokenize(new SourceFile(sourceFileContent));
        }
        private IEnumerable<IToken> TokenizeInternal()
        {
            while (!_ch.IsEOF() && _index < _sourceFile.Contents.Length -1)
                yield return ParseNextToken();

            yield return CreateToken(TokenType.EOF);
        }
        private IToken ParseNextToken()
        {
            if (_ch.IsEOF())
            {
                return CreateToken(TokenType.EOF);
            }
            else if (_ch.IsNewLine())
            {
                return NewLine();
            }
            else if (_ch.IsWhiteSpace())
            {
                return WhiteSpace();
            }
            else if (_ch.IsDigit())
            {
                return Int();
            }
            else if (_ch == '/' && (_next == '/' || _next == '*'))
            {
                return Comment();
            }
            else if (_ch.IsLetter() || _ch == '_')
            {
                return Identifier();
            }
            else if (_ch == '"')
            {
                return StringLiteral();
            }
            else if (_ch == '.' && _next.IsDigit())
            {
                return Real();
            }
            else if (_ch.IsPunctuation())
            {
                return Punctuation();
            }
            else
            {
                // NOTE(Dan): This will be an error.
                return Error();
            }
        }
        private IToken CreateToken(TokenType tokenType)
        {
            var content = _builder.ToString();
            var startSourceLocation = _sourceFileLocation;
            var endSourceLocation = new SourceFileLocation(_column, _index, _line);

            _sourceFileLocation = endSourceLocation;
            _builder.Clear();

            return new Token(tokenType, content, startSourceLocation, endSourceLocation);
        }
        private char Peak(int ahead)
        {
            if (_index + ahead > _sourceFile.Contents.Length)
                throw new IndexOutOfRangeException("Cannot peak past the end of the content");

            return _sourceFile.Contents[_index + ahead];
        }
        private void Consume()
        {
            _builder.Append(_ch);
            Advance();
        }
        private void Advance()
        {
            _index++;
            _column++;
        }
        private IToken NewLine()
        {
            Consume();

            _line++;
            _column = 1;

            return CreateToken(TokenType.NewLine);
        }
        private IToken WhiteSpace()
        {
            while (_ch.IsWhiteSpace())
                Consume();

            return CreateToken(TokenType.Whitespace);
        }
        private IToken Int()
        {
            while (_ch.IsDigit())
                Consume();

            // Float
            if (_ch == 'f' || _ch == 'F' || _ch == 'd' || _ch == 'D' || _ch == 'm' || _ch == 'M' || _ch == '.' || _ch == 'e')
            {
                return Real();
            }

            if (!_ch.IsWhiteSpace() && !_ch.IsPunctuation() && !_ch.IsEOF())
            {
                return Error();
            }

            return CreateToken(TokenType.IntegerLiteral);
        }
        private IToken Real()
        {
            if (_ch == 'f' || _ch == 'F' || _ch == 'd' || _ch == 'D' || _ch == 'm' || _ch == 'M')
            {
                Advance();

                if ((!_ch.IsWhiteSpace() && !_ch.IsPunctuation() && !_ch.IsEOF()) || _ch == '.')
                {
                    return Error(message: $"Remove '{_ch}' in real number.");
                }

                return CreateToken(TokenType.RealLiteral);
            }

            int preDotLength = _index - _sourceFileLocation.Index;

            if (_ch == '.')
            {
                Consume();
            }

            while (_ch.IsDigit())
            {
                Consume();
            }

            if (Peak(-1) == '.')
            {
                // .e10 is invalid.
                return Error(message: "Must contain digits after '.'");
            }

            if (_ch == 'e')
            {
                Consume();
                if (preDotLength > 1)
                {
                    return Error(message: "Coefficient must be less than 10.");
                }

                if (_ch == '+' || _ch == '-')
                {
                    Consume();
                }
                while (_ch.IsDigit())
                {
                    Consume();
                }
            }

            if (_ch == 'f' || _ch == 'F' || _ch == 'd' || _ch == 'D' || _ch == 'm' || _ch == 'M')
            {
                Consume();
            }

            if (!_ch.IsWhiteSpace() && !_ch.IsPunctuation() && !_ch.IsEOF())
            {
                if (_ch.IsLetter())
                {
                    return Error(message: "'{0}' is an invalid real value");
                }

                return Error();
            }

            return CreateToken(TokenType.RealLiteral);
        }
        private IToken Double()
        {
            throw new NotImplementedException();
        }
        private IToken Decimal()
        {
            throw new NotImplementedException();
        }
        private IToken Punctuation()
        {
            switch (_ch)
            {
                case ';':
                    Consume();
                    return CreateToken(TokenType.Semicolon);

                case ':':
                    Consume();
                    return CreateToken(TokenType.Colon);

                case '{':
                    Consume();
                    return CreateToken(TokenType.LeftBracket);

                case '}':
                    Consume();
                    return CreateToken(TokenType.RightBracket);

                case '[':
                    Consume();
                    return CreateToken(TokenType.LeftBrace);

                case ']':
                    Consume();
                    return CreateToken(TokenType.RightBrace);

                case '(':
                    Consume();
                    return CreateToken(TokenType.LeftParenthesis);

                case ')':
                    Consume();
                    return CreateToken(TokenType.RightParenthesis);

                case '>':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.GreaterThanOrEqual);
                    }
                    else if (_ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenType.BitShiftRight);
                    }
                    return CreateToken(TokenType.GreaterThan);

                case '<':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.LessThanOrEqual);
                    }
                    else if (_ch == '<')
                    {
                        Consume();
                        return CreateToken(TokenType.BitShiftLeft);
                    }
                    return CreateToken(TokenType.LessThan);

                case '+':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.PlusEqual);
                    }
                    else if (_ch == '+')
                    {
                        Consume();
                        return CreateToken(TokenType.PlusPlus);
                    }
                    return CreateToken(TokenType.Plus);

                case '-':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.MinusEqual);
                    }
                    else if (_ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenType.Arrow);
                    }
                    else if (_ch == '-')
                    {
                        Consume();
                        return CreateToken(TokenType.MinusMinus);
                    }
                    return CreateToken(TokenType.Minus);

                case '=':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.Equal);
                    }
                    else if (_ch == '>')
                    {
                        Consume();
                        return CreateToken(TokenType.FatArrow);
                    }
                    return CreateToken(TokenType.Assignment);

                case '!':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.NotEqual);
                    }
                    return CreateToken(TokenType.Not);

                case '*':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.MulEqual);
                    }
                    return CreateToken(TokenType.Mul);

                case '/':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.DivEqual);
                    }
                    return CreateToken(TokenType.Div);

                case '.':
                    Consume();
                    return CreateToken(TokenType.Dot);

                case ',':
                    Consume();
                    return CreateToken(TokenType.Comma);

                case '&':
                    Consume();
                    if (_ch == '&')
                    {
                        Consume();
                        return CreateToken(TokenType.BooleanAnd);
                    }
                    else if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.BitwiseAndEqual);
                    }
                    return CreateToken(TokenType.BitwiseAnd);

                case '|':
                    Consume();
                    if (_ch == '|')
                    {
                        Consume();
                        return CreateToken(TokenType.BooleanOr);
                    }
                    else if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.BitwiseOrEqual);
                    }
                    return CreateToken(TokenType.BitwiseOr);

                case '%':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.ModEqual);
                    }
                    return CreateToken(TokenType.Mod);

                case '^':
                    Consume();
                    if (_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenType.BitwiseXorEqual);
                    }
                    return CreateToken(TokenType.BitwiseXor);

                case '?':
                    Consume();
                    if (_ch == '?')
                    {
                        Consume();
                        return CreateToken(TokenType.DoubleQuestion);
                    }

                    return CreateToken(TokenType.Question);

                default:
                    return Error();
            }
        }
        private IToken Comment()
        {
            Consume();

            if (_ch == '*')
                return BlockComment();

            Consume();

            while (!_ch.IsNewLine() && !_ch.IsEOF())
                Consume();

            return CreateToken(TokenType.LineComment);
        }
        private IToken BlockComment()
        {
            Func<bool> IsEndOfComment = () => _ch == '*' && _next == '/';
            while (!IsEndOfComment())
            {
                if (_ch.IsEOF())
                {
                    return CreateToken(TokenType.Error);
                }
                //if (_ch.IsNewLine())
                //{
                //    NewLine();
                //}
                Consume();
            }

            Consume();
            Consume();

            return CreateToken(TokenType.BlockComment);
        }
        private IToken Identifier()
        {
            while (_ch.IsIdentifier())
            {
                Consume();
            }

            if (!_ch.IsWhiteSpace() && !_ch.IsPunctuation() && !_ch.IsEOF())
            {
                return Error();
            }

            if (_builder.ToString().IsKeyword(_keywords))
            {
                return CreateToken(TokenType.Keyword);
            }

            return CreateToken(TokenType.Identifier);
        }
        private IToken StringLiteral()
        {
            Advance();

            // TODO(Dan): Do we need to consider escaping here?
            while (_ch != '"')
            {
                if (_ch.IsEOF())
                {
                    AddError("Unexpected End Of File", Severity.Fatal);
                    return CreateToken(TokenType.Error);
                }
                Consume();
            }

            Advance();

            return CreateToken(TokenType.StringLiteral);
        }
        private IToken Error(Severity severity = Severity.Error, string message = "Unexpected token '{0}'")
        {
            while (!_ch.IsWhiteSpace() && !_ch.IsEOF() && !_ch.IsPunctuation())
                Consume();

            AddError(string.Format(message, severity), severity);

            return CreateToken(TokenType.Error);
        }
        private void AddError(string message, Severity severity)
        {
            var sourcePart = new SourceFilePart(_sourceFileLocation, new SourceFileLocation(_column, _index, _line), _builder.ToString().Split('\n'));
            _errorSink.AddError(message, sourcePart, severity);
        }

        public SxcLexer(string[] grammar) : this(grammar, new ErrorSink()) { }
        public SxcLexer(string[] grammar, IErrorSink errorSink)
        {
            if (errorSink == null)
                throw new ArgumentNullException(nameof(errorSink));

            _builder = new StringBuilder();
            _sourceFile = null;
            _errorSink = errorSink;
            _keywords = grammar;
        }
    }
}
