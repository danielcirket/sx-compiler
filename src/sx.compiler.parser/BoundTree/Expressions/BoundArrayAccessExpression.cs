using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundArrayAccessExpression : BoundExpression
    {
        public BoundArrayAccessExpression(ArrayAccessExpression arrayExpression)
            : base(arrayExpression)
        {
        }
    }
}
