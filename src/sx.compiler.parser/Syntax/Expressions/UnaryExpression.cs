using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

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
        public UnaryExpression(ISourceFilePart span, Expression argument, UnaryOperator op, Declaration binding, Scope scope) : base(span, binding, scope)
        {
            Argument = argument;
            Operator = op;
        }
        public UnaryExpression(UnaryExpression expression, Expression argument, Declaration binding, Scope scope)
            : this(expression.FilePart, argument, expression.Operator, binding, scope)
        {

        }
    }
}
