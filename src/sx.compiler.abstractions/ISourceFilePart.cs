namespace Sx.Compiler.Abstractions
{
    public interface ISourceFilePart
    {
        string FileName { get; }
        ISourceFileLocation Start { get; }
        ISourceFileLocation End { get; }
        string[] Lines { get; }
    }
}
