using System;

namespace Sxc.Lexer.Abstractions
{
    public interface IToken : IEquatable<IToken>
    {
        TokenCategory Category { get; }
        TokenType TokenType { get; }
        ISourceFilePart SourceFilePart { get; }
        string Value { get; }
    }
}
