using System;
using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ModuleDeclaration : Declaration
    {
        public IEnumerable<ClassDeclaration> Classes { get; }
        public IEnumerable<MethodDeclaration> Methods { get; }
        public override SyntaxKind Kind => SyntaxKind.ModuleDeclaration;

        public ModuleDeclaration(ISourceFilePart span, string name, IEnumerable<ClassDeclaration> classes, IEnumerable<MethodDeclaration> methods) : base(span, name)
        {
            Classes = classes;
            Methods = methods;
        }
        public ModuleDeclaration(ISourceFilePart span, string name, IEnumerable<ClassDeclaration> classes, IEnumerable<MethodDeclaration> methods, Scope scope) : base(span, name, scope)
        {
            Classes = classes;
            Methods = methods;
        }
        public ModuleDeclaration(ModuleDeclaration declaration, IEnumerable<ClassDeclaration> classes, IEnumerable<MethodDeclaration> methods, Scope scope)
            : this(declaration.FilePart, declaration.Name, classes, methods, scope)
        {
        }
    }
}
