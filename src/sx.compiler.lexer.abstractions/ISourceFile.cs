namespace Sx.Lexer.Abstractions
{
    public interface ISourceFile
    {
        string Contents { get; }
        string[] Lines { get; }
    }
}
