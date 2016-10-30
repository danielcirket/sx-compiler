using System;

namespace Sxc.Lexer.Abstractions
{
    public interface ISourceFileLocation : IEquatable<ISourceFileLocation>
    {
        int Column { get; }

        int Index { get; }

        int Line { get; }
    }
}
