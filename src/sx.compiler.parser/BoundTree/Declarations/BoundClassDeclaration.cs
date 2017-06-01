using System;
using System.Collections.Generic;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public class BoundClassDeclaration : BoundDeclaration
    {
        private readonly ClassDeclaration _syntaxNode;

        public IEnumerable<BoundFieldDeclaration> Fields { get; }
        public IEnumerable<BoundPropertyDeclaration> Properties { get; }
        public IEnumerable<BoundMethodDeclaration> Methods { get; }
        public IEnumerable<BoundConstructorDeclaration> Constructors { get; }
        public SymbolTable SymbolTable { get; }

        public BoundClassDeclaration(ClassDeclaration node, 
            IEnumerable<BoundFieldDeclaration> fields,
            IEnumerable<BoundPropertyDeclaration> properties,
            IEnumerable<BoundMethodDeclaration> methods,
            IEnumerable<BoundConstructorDeclaration> constructors,
            SymbolTable symbolTable)
            : base(node.Name, node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));
            if (methods == null)
                throw new ArgumentNullException(nameof(methods));
            if (constructors == null)
                throw new ArgumentNullException(nameof(constructors));
            if (symbolTable == null)
                throw new ArgumentNullException(nameof(symbolTable));

            _syntaxNode = node;
            Fields = fields;
            Properties = properties;
            Methods = methods;
            Constructors = constructors;
            SymbolTable = symbolTable;
        }
    }
}
