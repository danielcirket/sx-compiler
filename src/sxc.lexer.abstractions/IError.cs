namespace Sxc.Lexer.Abstractions
{
    public interface IError
    {
        string[] Lines { get; }
        string Message { get; }
        Severity Severity { get; }
        ISourceFilePart FilePart { get; }
    }
}
