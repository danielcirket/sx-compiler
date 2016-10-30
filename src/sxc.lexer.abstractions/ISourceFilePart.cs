namespace Sxc.Lexer.Abstractions
{
    public interface ISourceFilePart
    {
        ISourceFileLocation Start { get; }
        ISourceFileLocation End { get; }
        string[] Lines { get; }
    }
}
