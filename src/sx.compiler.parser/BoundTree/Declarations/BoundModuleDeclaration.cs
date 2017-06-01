using System;
using System.Collections.Generic;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundModuleDeclaration : BoundDeclaration
    {
        private readonly ModuleDeclaration _syntaxNode;

        public IEnumerable<BoundClassDeclaration> Classes { get; }
        public IEnumerable<BoundMethodDeclaration> Methods { get; }
        public SymbolTable SymbolTable { get; }

        public BoundModuleDeclaration(ModuleDeclaration node,
            IEnumerable<BoundClassDeclaration> classes,
            IEnumerable<BoundMethodDeclaration> methods,
            SymbolTable symbolTable)
            : base(node.Name, node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (classes == null)
                throw new ArgumentNullException(nameof(classes));
            if (methods == null)
                throw new ArgumentNullException(nameof(methods));
            if (symbolTable == null)
                throw new ArgumentNullException(nameof(symbolTable));

            _syntaxNode = node;
            Classes = classes;
            Methods = methods;
            SymbolTable = symbolTable;
        }
    }
}
