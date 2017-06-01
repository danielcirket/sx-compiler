using Sx.Compiler.Abstractions;

namespace Sx.Lexer
{
    internal class SourceFilePart : ISourceFilePart
    {
        public string FileName { get; }
        public ISourceFileLocation Start { get; }
        public ISourceFileLocation End { get; }
        public string[] Lines { get; }

        public SourceFilePart(string fileName, ISourceFileLocation start, ISourceFileLocation end, string[] lines)
        {
            FileName = fileName;
            Start = start;
            End = end;
            Lines = lines;
        }
    }
}
