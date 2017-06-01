using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundParameterDeclaration : BoundDeclaration
    {
        public SymbolTable SymbolTable { get; }

        public BoundParameterDeclaration(ParameterDeclaration parameter, SymbolTable symbolTable)
            : base(parameter.Name, parameter)
        {
            SymbolTable = symbolTable;
        }
    }
}
