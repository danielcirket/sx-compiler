using Sx.Lexer.Abstractions;

namespace Sx.Lexer
{
    internal class SourceFilePart : ISourceFilePart
    {
        public ISourceFileLocation Start { get; }
        public ISourceFileLocation End { get; }
        public string[] Lines { get; }

        public SourceFilePart(ISourceFileLocation start, ISourceFileLocation end, string[] lines)
        {
            Start = start;
            End = end;
            Lines = lines;
        }
    }
}
