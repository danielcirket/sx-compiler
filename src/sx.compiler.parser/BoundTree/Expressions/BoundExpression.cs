using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public abstract class BoundExpression : BoundNode
    {
        //public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public BoundExpression Left { get; }
        public BoundBinaryOperator Operator { get; }
        public BoundExpression Right { get; }

        protected BoundExpression(Expression expression)
            : base(expression)
        {
        }
    }
}
