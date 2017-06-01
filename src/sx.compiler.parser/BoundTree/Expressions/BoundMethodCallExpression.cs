using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundMethodCallExpression : BoundExpression
    {
        public BoundMethodCallExpression(MethodCallExpression expression)
            : base(expression)
        {
        }
    }
}
