using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(UnaryExpression expression)
            : base(expression)
        {
        }
    }
}
