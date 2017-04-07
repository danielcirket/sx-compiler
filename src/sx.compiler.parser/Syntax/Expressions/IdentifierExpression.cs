using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }
        public override SyntaxKind Kind => SyntaxKind.IdentifierExpression;

        public IdentifierExpression(ISourceFilePart span, string identifier) : base(span)
        {
            Identifier = identifier;
        }
    }
}
