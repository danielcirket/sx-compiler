using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

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
        public MethodCallExpression(ISourceFilePart span, Expression reference, IEnumerable<Expression> arguments, Declaration binding, Scope scope)
            : base(span, binding, scope)
        {
            Reference = reference;
            Arguments = arguments;
        }
        public MethodCallExpression(MethodCallExpression expression, Expression reference, IEnumerable<Expression> arguments, Scope scope)
            : this(expression.FilePart, reference, arguments, null, scope)
        {

        }
        public MethodCallExpression(MethodCallExpression expression, Expression reference, IEnumerable<Expression> arguments, Declaration binding, Scope scope)
            : this(expression.FilePart, reference, arguments, binding, scope)
        {

        }
    }
}
