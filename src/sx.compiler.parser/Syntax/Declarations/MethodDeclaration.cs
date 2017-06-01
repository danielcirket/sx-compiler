﻿using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class MethodDeclaration : Declaration
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.MethodDeclaration;
        public DeclarationVisibility Visibility { get; }
        public IEnumerable<ParameterDeclaration> Parameters { get; }
        public TypeDeclaration ReturnType { get; }
        public IEnumerable<TypeDeclaration> TypeDeclarations { get; }

        public MethodDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibility, TypeDeclaration returnType, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : this(span, name, visibility, returnType, Enumerable.Empty<TypeDeclaration>(), parameters, body)
        { 
        }
        public MethodDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibility, TypeDeclaration returnType, IEnumerable<TypeDeclaration> typeDeclarations, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, name)
        {
            Visibility = visibility;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
            TypeDeclarations = typeDeclarations;
        }
        public MethodDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibility, TypeDeclaration returnType, IEnumerable<TypeDeclaration> typeDeclarations, IEnumerable<ParameterDeclaration> parameters, BlockStatement body, Scope scope) : base(span, name, scope)
        {
            Visibility = visibility;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
            TypeDeclarations = typeDeclarations;
        }
        public MethodDeclaration(MethodDeclaration declaration, IEnumerable<ParameterDeclaration> parameters, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Visibility, declaration.ReturnType, declaration.TypeDeclarations, parameters, declaration.Body, scope)
        {

        }
        public MethodDeclaration(MethodDeclaration declaration, IEnumerable<ParameterDeclaration> parameters, BlockStatement body, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Visibility, declaration.ReturnType, declaration.TypeDeclarations, parameters, body, scope)
        {

        }
    }
}
