namespace Sx.Compiler.Abstractions
{
    public interface ISourceFile
    {
        string Name { get; }
        string Contents { get; }
        string[] Lines { get; }
    }
}
