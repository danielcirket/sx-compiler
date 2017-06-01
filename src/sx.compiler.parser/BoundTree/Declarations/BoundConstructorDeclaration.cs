using System.Collections.Generic;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundConstructorDeclaration : BoundDeclaration
    {
        public IEnumerable<BoundParameterDeclaration> Parameters { get; }
        public SymbolTable SymbolTable { get; }

        public BoundConstructorDeclaration(ConstructorDeclaration constructor, IEnumerable<BoundParameterDeclaration> boundParameters, SymbolTable symbolTable)
            : base(constructor.Name, constructor)
        {
            Parameters = boundParameters;
            SymbolTable = symbolTable;
        }
    }
}
