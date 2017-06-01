using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

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
        public PropertyDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibilty, TypeDeclaration type, MethodDeclaration getMethod, MethodDeclaration setMethod, Scope scope) : base(span, name, scope)
        {
            Visibility = visibilty;
            Type = type;
            GetMethod = getMethod;
            SetMethod = setMethod;
        }
        public PropertyDeclaration(PropertyDeclaration declaration, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Visibility, declaration.Type, declaration.GetMethod, declaration.SetMethod, scope)
        {

        }
        public PropertyDeclaration(PropertyDeclaration declaration, MethodDeclaration getMethod, MethodDeclaration setMethod, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Visibility, declaration.Type, getMethod, setMethod, scope)
        {

        }
    }
}
