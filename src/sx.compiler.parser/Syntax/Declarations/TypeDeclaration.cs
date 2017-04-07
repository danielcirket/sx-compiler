using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class TypeDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.TypeDeclaration;

        public TypeDeclaration(ISourceFilePart span, string name) : base(span, name)
        {

        }
    }
}
