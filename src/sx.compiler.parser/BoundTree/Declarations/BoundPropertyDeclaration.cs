using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundPropertyDeclaration : BoundDeclaration
    {
        public SymbolTable SymbolTable { get; }

        public BoundPropertyDeclaration(PropertyDeclaration propertyDeclaration, SymbolTable symbolTable)
            : base(propertyDeclaration.Name, propertyDeclaration)
        {
            SymbolTable = symbolTable;
        }
    }
}
