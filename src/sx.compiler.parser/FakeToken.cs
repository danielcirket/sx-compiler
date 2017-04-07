using Sx.Compiler.Abstractions;
using Sx.Lexer.Abstractions;

namespace Sx.Compiler.Parser
{
    public class FakeToken : IToken
    {
        public TokenCategory Category => TokenCategory.Unknown;
        public ISourceFilePart SourceFilePart { get; }
        public TokenType TokenType { get; }
        public string Value { get; }
        public bool IsTrivia() => (Category == TokenCategory.Whitespace || Category == TokenCategory.Comment);

        public FakeToken(ISourceFilePart span, TokenType type, string value)
        {
            SourceFilePart = span;
            TokenType = type;
            Value = value;
        }
    }
}
