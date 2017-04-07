using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class ReferenceExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.ReferenceExpression;
        public IEnumerable<Expression> References { get; }

        public ReferenceExpression(ISourceFilePart span, IEnumerable<Expression> references) : base(span)
        {
            References = references;
        }
    }
}
