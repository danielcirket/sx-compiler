using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

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
        public ArrayAccessExpression(ArrayAccessExpression expression, Expression reference, IEnumerable<Expression> arguments, Declaration binding, Scope scope) 
            : base(expression.FilePart, binding, scope)
        {
            Reference = reference;
            Arguments = arguments;
        }
    }
}
