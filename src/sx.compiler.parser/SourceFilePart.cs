using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser
{
    internal class SourceFilePart : ISourceFilePart
    {
        public ISourceFileLocation Start { get; }
        public ISourceFileLocation End { get; }
        public string[] Lines { get; }
        public int Length => End.Index - Start.Index;

        public override string ToString()
        {
            return $"{Start.Line} {Start.Column} {Length}";
        }

        public SourceFilePart(ISourceFileLocation start, ISourceFileLocation end)
        {
            Start = start;
            End = end;
        }
    }
}
