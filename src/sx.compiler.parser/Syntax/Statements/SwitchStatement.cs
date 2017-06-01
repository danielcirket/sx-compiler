using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class SwitchStatement : Statement
    {
        public Expression Condition { get; }
        public IEnumerable<CaseStatement> Cases { get; }
        public override SyntaxKind Kind => SyntaxKind.SwitchStatement;

        public SwitchStatement(ISourceFilePart span, Expression condition, IEnumerable<CaseStatement> cases) : base(span)
        {
            Condition = condition;
            Cases = cases;
        }
        public SwitchStatement(ISourceFilePart span, Expression condition, IEnumerable<CaseStatement> cases, Scope scope) : base(span, scope)
        {
            Condition = condition;
            Cases = cases;
        }
        public SwitchStatement(SwitchStatement statement, Expression condition, IEnumerable<CaseStatement> cases, Scope scope) 
            : this(statement.FilePart, condition, cases, scope)
        {
        }
    }
}
