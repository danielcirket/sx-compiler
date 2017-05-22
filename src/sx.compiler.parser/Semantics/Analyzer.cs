using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics.Passes.Declarations;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser.Semantics
{
    public class SemanticAnalyzer
    {
        private readonly IErrorSink _errorSink;
        private readonly ISemanticPass[] _passes = new[]
        {
            new DeclarationPass(),
            //new TypeCheckPass(),
        };

        public IErrorSink ErrorSink => _errorSink;

        public SemanticAnalyzer(IErrorSink errorSink, CompilationUnit compilationUnit)
        {
            _errorSink = errorSink;

            foreach (var pass in _passes)
            {
                pass.Run(errorSink, compilationUnit);

                // TODO(Dan): E.g. too many errors etc etc
                if (!pass.ShouldContinue)
                {
                    
                }
            }
        }
    }
}
