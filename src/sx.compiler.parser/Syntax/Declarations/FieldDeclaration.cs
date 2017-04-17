using Sx.Compiler.Abstractions;
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
    }
}
