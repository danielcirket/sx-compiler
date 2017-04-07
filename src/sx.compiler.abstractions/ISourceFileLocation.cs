using System;

namespace Sx.Compiler.Abstractions
{
    public interface ISourceFileLocation : IEquatable<ISourceFileLocation>
    {
        int Column { get; }

        int Index { get; }

        int Line { get; }
    }
}
