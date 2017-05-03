using System;
using Sx.Compiler.Abstractions;
using Sx.Lexer.Abstractions;

namespace Sx.Lexer
{
    public class Token : IToken
    {
        private TokenCategory _category;
        private TokenType _type;
        private ISourceFilePart _sourceFilePart;
        private string _value;

        public TokenCategory Category => GetTokenCategory();
        public ISourceFilePart SourceFilePart => _sourceFilePart;
        public TokenType TokenType => _type;
        public string Value => _value;

        public override string ToString()
        {
            return $@"Start: (Line){_sourceFilePart.Start.Line} (Col){_sourceFilePart.Start.Column}, Type: {TokenType}, Value: {Value}";
        }
        private TokenCategory GetTokenCategory()
        {
            switch (TokenType)
            {
                case TokenType.Arrow:
                case TokenType.FatArrow:
                case TokenType.Colon:
                case TokenType.Semicolon:
                case TokenType.Comma:
                case TokenType.Dot:
                    return TokenCategory.Punctuation;

                case TokenType.Equal:
                case TokenType.NotEqual:
                case TokenType.Not:
                case TokenType.LessThan:
                case TokenType.LessThanOrEqual:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.Minus:
                case TokenType.MinusEqual:
                case TokenType.MinusMinus:
                case TokenType.Mod:
                case TokenType.ModEqual:
                case TokenType.Mul:
                case TokenType.MulEqual:
                case TokenType.Plus:
                case TokenType.PlusEqual:
                case TokenType.PlusPlus:
                case TokenType.Question:
                case TokenType.DoubleQuestion:
                case TokenType.DivEqual:
                case TokenType.Div:
                case TokenType.BooleanOr:
                case TokenType.BooleanAnd:
                case TokenType.BitwiseXorEqual:
                case TokenType.BitwiseXor:
                case TokenType.BitwiseOrEqual:
                case TokenType.BitwiseOr:
                case TokenType.BitwiseAndEqual:
                case TokenType.BitwiseAnd:
                case TokenType.BitShiftLeft:
                case TokenType.BitShiftRight:
                case TokenType.Assignment:
                    return TokenCategory.Operator;

                case TokenType.BlockComment:
                case TokenType.LineComment:
                    return TokenCategory.Comment;

                case TokenType.NewLine:
                case TokenType.Whitespace:
                    return TokenCategory.Whitespace;

                case TokenType.LeftBrace:
                case TokenType.LeftBracket:
                case TokenType.LeftParenthesis:
                case TokenType.RightBrace:
                case TokenType.RightBracket:
                case TokenType.RightParenthesis:
                    return TokenCategory.Grouping;

                case TokenType.Identifier:
                case TokenType.Keyword:
                    return TokenCategory.Identifier;

                case TokenType.StringLiteral:
                case TokenType.IntegerLiteral:
                case TokenType.RealLiteral:
                    return TokenCategory.Constant;

                case TokenType.Error:
                    return TokenCategory.Invalid;

                default: return TokenCategory.Unknown;
            }
        }

        public Token(TokenType tokenType, string content, ISourceFileLocation startSourceLocation, ISourceFileLocation endSourceLocation)
        {
            _type = tokenType;
            _sourceFilePart = new SourceFilePart(startSourceLocation, endSourceLocation, content.Split('\n'));
            _value = content;
        }
    }
}
