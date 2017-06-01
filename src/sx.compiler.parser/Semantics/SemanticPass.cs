using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Semantics
{
    public interface ISemanticPass
    {
        bool ShouldContinue { get; }
        void Run(IErrorSink errorSink, ref CompilationUnit compilationUnit);
    }
}
