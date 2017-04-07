using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class UnaryExpression : Expression
    {
        public Expression Argument { get; }
        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public UnaryOperator Operator { get; }

        public UnaryExpression(ISourceFilePart span, Expression argument, UnaryOperator op) : base(span)
        {
            Argument = argument;
            Operator = op;
        }
    }
}
