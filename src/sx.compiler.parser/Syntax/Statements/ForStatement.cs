using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class ForStatement : Statement
    {
        public BlockStatement Body { get; }
        public Expression Condition { get; }
        public Expression Increment { get; }
        public SyntaxNode Initialization { get; }
        public override SyntaxKind Kind => SyntaxKind.ForStatement;

        public ForStatement(ISourceFilePart span, SyntaxNode initialization, Expression condition, Expression increment, BlockStatement body) : base(span)
        {
            Initialization = initialization;
            Condition = condition;
            Increment = increment;
            Body = body;
        }
    }
}
