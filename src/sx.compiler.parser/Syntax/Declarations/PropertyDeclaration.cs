using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class PropertyDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.PropertyDeclaration;
        public MethodDeclaration GetMethod { get; }
        public MethodDeclaration SetMethod { get; }
        public TypeDeclaration Type { get; }

        public PropertyDeclaration(ISourceFilePart span, string name, TypeDeclaration type, MethodDeclaration getMethod, MethodDeclaration setMethod) : base(span, name)
        {
            Type = type;
            GetMethod = getMethod;
            SetMethod = setMethod;
        }
    }
}
