using System;

namespace Sx.Lexer.Abstractions
{
    public interface ISourceFileLocation : IEquatable<ISourceFileLocation>
    {
        int Column { get; }

        int Index { get; }

        int Line { get; }
    }
}
