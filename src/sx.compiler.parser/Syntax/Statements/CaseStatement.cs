using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class CaseStatement : Statement
    {
        public IEnumerable<SyntaxNode> Body { get; }
        public IEnumerable<Expression> Cases { get; }
        public override SyntaxKind Kind => SyntaxKind.CaseStatement;

        public CaseStatement(ISourceFilePart span, IEnumerable<Expression> cases, IEnumerable<SyntaxNode> body) : base(span)
        {
            Body = body;
            Cases = cases;
        }
    }
}
