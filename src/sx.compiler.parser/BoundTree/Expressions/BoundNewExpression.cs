using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundNewExpression : BoundExpression
    {
        public BoundNewExpression(NewExpression expression)
            : base(expression)
        {
        }
    }
}
