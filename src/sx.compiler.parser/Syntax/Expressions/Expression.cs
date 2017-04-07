using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public abstract class Expression : SyntaxNode
    {
        public override SyntaxCatagory Category => SyntaxCatagory.Expression;

        protected Expression(ISourceFilePart span) : base(span)
        {
        }
    }
}
