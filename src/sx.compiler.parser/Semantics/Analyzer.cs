using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics.Passes.Declarations;

namespace Sx.Compiler.Parser.Semantics
{
    public class SemanticAnalyzer
    {
        private readonly IErrorSink _errorSink;
        private readonly ISemanticPass[] _passes = new ISemanticPass[]
        {
            new ForwardDeclarationPass(),
            new DeclarationPass(),
            //new TypeResolutionPass(),
            //new TypeInferencePass(),
            //new TypeCheckPass(),
        };

        public IErrorSink ErrorSink => _errorSink;

        public SemanticAnalyzer(IErrorSink errorSink, CompilationUnit compilationUnit)
        {
            _errorSink = errorSink;

            foreach (var pass in _passes)
            {
                pass.Run(errorSink, ref compilationUnit);

                // TODO(Dan): E.g. too many errors etc etc
                if (!pass.ShouldContinue)
                {
                    // TODO(Dan): Format and output errors!
                }
            }
        }
    }
}
