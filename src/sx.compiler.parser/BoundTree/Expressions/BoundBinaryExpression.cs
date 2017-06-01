using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BinaryExpression expression)
            : base(expression)
        {
        }
    }
}
