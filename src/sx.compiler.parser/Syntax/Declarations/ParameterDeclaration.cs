using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ParameterDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.ParameterDeclaration;
        public TypeDeclaration Type { get; }

        public ParameterDeclaration(ISourceFilePart span, string name, TypeDeclaration type) : base(span, name)
        {
            Type = type;
        }
        public ParameterDeclaration(ISourceFilePart span, string name, TypeDeclaration type, Scope scope) : base(span, name, scope)
        {
            Type = type;
        }
        public ParameterDeclaration(ParameterDeclaration declaration, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Type, scope)
        {

        }
    }
}
