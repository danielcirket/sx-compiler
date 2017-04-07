using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class VariableDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
        public TypeDeclaration Type { get; }
        public Expression Value { get; }

        public VariableDeclaration(ISourceFilePart span, string name, TypeDeclaration type, Expression value) : base(span, name)
        {
            Type = type;
            Value = value;
        }
    }
}
