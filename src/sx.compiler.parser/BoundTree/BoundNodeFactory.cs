using System.Collections.Generic;
using Sx.Compiler.Parser.BoundTree.Declarations;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree
{
    public static class BoundNodeFactory
    {
        public static BoundModuleDeclaration BindModuleDeclaration(ModuleDeclaration node,
            IEnumerable<BoundClassDeclaration> classes,
            IEnumerable<BoundMethodDeclaration> methods,
            SymbolTable symbolTable)
        {
            return new BoundModuleDeclaration(
                node,
                classes,
                methods,
                symbolTable);
        }

        public static BoundClassDeclaration BindClassDeclaration(ClassDeclaration node,
            IEnumerable<BoundFieldDeclaration> fields,
            IEnumerable<BoundPropertyDeclaration> properties,
            IEnumerable<BoundMethodDeclaration> methods,
            IEnumerable<BoundConstructorDeclaration> constructors,
            SymbolTable symbolTable)
        {
            return new BoundClassDeclaration(
                node,
                fields,
                properties,
                methods,
                constructors,
                symbolTable);
        }
    }
}
