using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class PropertyDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.PropertyDeclaration;
        public DeclarationVisibility Visibility { get; }
        public MethodDeclaration GetMethod { get; }
        public MethodDeclaration SetMethod { get; }
        public TypeDeclaration Type { get; }

        public PropertyDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibilty, TypeDeclaration type, MethodDeclaration getMethod, MethodDeclaration setMethod) : base(span, name)
        {
            Visibility = visibilty;
            Type = type;
            GetMethod = getMethod;
            SetMethod = setMethod;
        }
    }
}
