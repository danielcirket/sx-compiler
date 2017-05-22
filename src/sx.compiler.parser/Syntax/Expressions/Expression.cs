using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public abstract class Expression : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Expression;

        protected Expression(ISourceFilePart span) : base(span)
        {
        }
    }
}
