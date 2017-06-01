using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundTypeDeclaration : BoundDeclaration
    {
        public BoundTypeDeclaration(TypeDeclaration typeDeclaration)
            : base(typeDeclaration.Name, typeDeclaration)
        {

        }
    }
}
