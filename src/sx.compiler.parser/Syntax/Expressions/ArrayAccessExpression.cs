using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class ArrayAccessExpression : Expression
    {
        public IEnumerable<Expression> Arguments { get; }
        public override SyntaxKind Kind => SyntaxKind.ArrayAccessExpression;
        public Expression Reference { get; }

        public ArrayAccessExpression(ISourceFilePart span, Expression reference, IEnumerable<Expression> arguments) : base(span)
        {
            Reference = reference;
            Arguments = arguments;
        }
    }
}
