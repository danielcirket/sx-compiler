using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class MethodCallExpression : Expression
    {
        public IEnumerable<Expression> Arguments { get; }
        public override SyntaxKind Kind => SyntaxKind.MethodCallExpression;
        public Expression Reference { get; }

        public MethodCallExpression(ISourceFilePart span, Expression reference, IEnumerable<Expression> arguments)
            : base(span)
        {
            Reference = reference;
            Arguments = arguments;
        }
    }
}
