using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
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
        public CaseStatement(ISourceFilePart span, IEnumerable<Expression> cases, IEnumerable<SyntaxNode> body, Scope scope) : base(span, scope)
        {
            Body = body;
            Cases = cases;
        }
        public CaseStatement(CaseStatement statement, IEnumerable<Expression> cases, IEnumerable<SyntaxNode> body, Scope scope)
            : this(statement.FilePart, cases, body, scope)
        {
        }
    }
}
