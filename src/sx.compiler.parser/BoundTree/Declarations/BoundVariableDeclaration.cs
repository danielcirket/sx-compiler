using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundVariableDeclaration : BoundDeclaration
    {
        public BoundVariableDeclaration(VariableDeclaration variableDeclaration)
            : base(variableDeclaration.Name, variableDeclaration)
        {

        }
    }
}
