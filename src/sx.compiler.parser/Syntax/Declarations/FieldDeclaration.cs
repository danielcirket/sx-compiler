using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class FieldDeclaration : Declaration
    {
        public Expression DefaultValue { get; }
        public override SyntaxKind Kind => SyntaxKind.FieldDeclaration;
        public DeclarationVisibility Visibility { get; }
        public TypeDeclaration Type { get; }

        public FieldDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibility, TypeDeclaration type, Expression value) : base(span, name)
        {
            Visibility = visibility;
            Type = type;
            DefaultValue = value;
        }
        public FieldDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibility, TypeDeclaration type, Expression value, Scope scope) : base(span, name, scope)
        {
            Visibility = visibility;
            Type = type;
            DefaultValue = value;
        }
        public FieldDeclaration(FieldDeclaration declaration, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Visibility, declaration.Type, declaration.DefaultValue, scope)
        {

        }
        public FieldDeclaration(FieldDeclaration declaration, Expression defaultValue, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Visibility, declaration.Type, defaultValue, scope)
        {

        }
    }
}
