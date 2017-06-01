using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundFieldDeclaration : BoundDeclaration
    {
        public SymbolTable SymbolTable { get; }

        public BoundFieldDeclaration(FieldDeclaration fieldDeclaration, SymbolTable symbolTable)
            : base(fieldDeclaration.Name, fieldDeclaration)
        {
            SymbolTable = symbolTable;
        }
    }
}
