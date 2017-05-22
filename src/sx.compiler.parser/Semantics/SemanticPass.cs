using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser.Semantics
{
    public interface ISemanticPass
    {
        bool ShouldContinue { get; }

        void Run(IErrorSink errorSink, CompilationUnit compilationUnit);
    }
}
