using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser
{
    internal class SourceFilePart : ISourceFilePart
    {
        public string FileName { get; }
        public ISourceFileLocation Start { get; }
        public ISourceFileLocation End { get; }
        public string[] Lines { get; }
        public int Length => End.Index - Start.Index;

        public override string ToString()
        {
            return $"{Start.Line} {Start.Column} {Length}";
        }

        public SourceFilePart(string fileName, string[] content, ISourceFileLocation start, ISourceFileLocation end)
        {
            FileName = fileName;
            Lines = content;
            Start = start;
            End = end;
        }
    }
}
