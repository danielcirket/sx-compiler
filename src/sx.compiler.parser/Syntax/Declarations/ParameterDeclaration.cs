using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ParameterDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.ParameterDeclaration;
        public TypeDeclaration Type { get; }

        public ParameterDeclaration(ISourceFilePart span, string name, TypeDeclaration type) : base(span, name)
        {
            Type = type;
        }
    }
}
