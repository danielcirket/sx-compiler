using System.Collections.Generic;
using Sx.Compiler.Parser.BoundTree.Statements;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundMethodDeclaration : BoundDeclaration
    {
        public IEnumerable<BoundParameterDeclaration> Parameters { get; }
        public BoundBlockStatement Body { get; }
        public SymbolTable SymbolTable { get; }

        public BoundMethodDeclaration(MethodDeclaration node, IEnumerable<BoundParameterDeclaration> parameters, SymbolTable symbolTable)
            : base(node.Name, node)
        {
            Parameters = parameters;
            SymbolTable = symbolTable;
        }
        public BoundMethodDeclaration(MethodDeclaration node, IEnumerable<BoundParameterDeclaration> parameters, BoundBlockStatement body, SymbolTable symbolTable)
            : this(node, parameters, symbolTable)
        {
            Body = body;
        }
    }
}
