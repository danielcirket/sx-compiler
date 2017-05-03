using Sx.Compiler.Abstractions;

namespace Sx.Lexer.Abstractions
{
    public interface IToken
    {
        TokenCategory Category { get; }
        TokenType TokenType { get; }
        ISourceFilePart SourceFilePart { get; }
        string Value { get; }
    }
}
