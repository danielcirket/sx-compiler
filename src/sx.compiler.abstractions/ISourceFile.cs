namespace Sx.Compiler.Abstractions
{
    public interface ISourceFile
    {
        string Contents { get; }
        string[] Lines { get; }
    }
}
