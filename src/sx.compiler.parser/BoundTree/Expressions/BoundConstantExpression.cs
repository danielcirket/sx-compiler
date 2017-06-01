using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundConstantExpression : BoundExpression
    {
        public BoundConstantExpression(ConstantExpression expression)
            : base(expression)
        {
        }
    }
}
