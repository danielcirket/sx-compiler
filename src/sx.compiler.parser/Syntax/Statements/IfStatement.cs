using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class IfStatement : Statement
    {
        public BlockStatement Body { get; }
        public ElseStatement ElseStatement { get; }
        public override SyntaxKind Kind => SyntaxKind.IfStatement;
        public Expression Predicate { get; }

        public IfStatement(ISourceFilePart span, Expression predicate, BlockStatement body, ElseStatement elseStatement) : base(span)
        {
            Predicate = predicate;
            Body = body;
            ElseStatement = elseStatement;
        }
        public IfStatement(ISourceFilePart span, Expression predicate, BlockStatement body, ElseStatement elseStatement, Scope scope) : base(span, scope)
        {
            Predicate = predicate;
            Body = body;
            ElseStatement = elseStatement;
        }
        public IfStatement(IfStatement statement, Expression predicate, BlockStatement body, ElseStatement elseStatement, Scope scope)
            : this(statement.FilePart, predicate, body, elseStatement, scope)
        {
        }
    }
}
