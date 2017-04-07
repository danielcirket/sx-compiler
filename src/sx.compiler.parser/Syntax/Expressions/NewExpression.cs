using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class NewExpression : Expression
    {
        public IEnumerable<Expression> Arguments { get; }
        public override SyntaxKind Kind => SyntaxKind.NewExpression;
        public Expression Reference { get; }

        public NewExpression(ISourceFilePart span, Expression reference, IEnumerable<Expression> arguments) : base(span)
        {
            Reference = reference;
            Arguments = arguments;
        }
    }
}
