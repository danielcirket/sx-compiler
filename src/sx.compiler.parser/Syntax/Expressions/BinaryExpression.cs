using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class BinaryExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public Expression Left { get; }

        public BinaryOperator Operator { get; }

        public Expression Right { get; }

        public BinaryExpression(ISourceFilePart span, Expression left, Expression right, BinaryOperator op) : base(span)
        {
            Left = left;
            Right = right;
            Operator = op;
        }
    }
}
