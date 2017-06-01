using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ClassDeclaration : Declaration
    {
        public IEnumerable<ConstructorDeclaration> Constructors { get; }
        public IEnumerable<FieldDeclaration> Fields { get; }
        public DeclarationVisibility Visibility { get; }
        public override SyntaxKind Kind => SyntaxKind.ClassDeclaration;
        public IEnumerable<MethodDeclaration> Methods { get; }
        public IEnumerable<PropertyDeclaration> Properties { get; }
        public IEnumerable<TypeDeclaration> TypeDeclarations { get; }

        public ClassDeclaration(ISourceFilePart span, string name, DeclarationVisibility visiblilty, IEnumerable<ConstructorDeclaration> constructors,
                                IEnumerable<FieldDeclaration> fields,
                                IEnumerable<MethodDeclaration> methods,
                                IEnumerable<PropertyDeclaration> properties,
                                IEnumerable<TypeDeclaration> typeDeclarations)
            : base(span, name)
        {
            Visibility = visiblilty;
            Constructors = constructors;
            Fields = fields;
            Methods = methods;
            Properties = properties;
            TypeDeclarations = typeDeclarations;
        }
        public ClassDeclaration(ISourceFilePart span, string name, DeclarationVisibility visiblilty, IEnumerable<ConstructorDeclaration> constructors,
                                IEnumerable<FieldDeclaration> fields,
                                IEnumerable<MethodDeclaration> methods,
                                IEnumerable<PropertyDeclaration> properties,
                                IEnumerable<TypeDeclaration> typeDeclarations,
                                Scope scope)
            : base(span, name, scope)
        {
            Visibility = visiblilty;
            Constructors = constructors;
            Fields = fields;
            Methods = methods;
            Properties = properties;
            TypeDeclarations = typeDeclarations;
        }
        public ClassDeclaration(ClassDeclaration declartion, Scope scope)
            : this(declartion.FilePart, declartion.Name, declartion.Visibility, declartion.Constructors,
                  declartion.Fields, declartion.Methods, declartion.Properties, declartion.TypeDeclarations,
                  scope)
        {

        }
        public ClassDeclaration(ClassDeclaration declartion, IEnumerable<FieldDeclaration> fields,
            IEnumerable<PropertyDeclaration> properties, IEnumerable<MethodDeclaration> methods,
            IEnumerable<ConstructorDeclaration> constructors, Scope scope)
            : this(declartion.FilePart, declartion.Name, declartion.Visibility, constructors,
                  fields, methods, properties, declartion.TypeDeclarations,
                  scope)
        {

        }
    }
}
