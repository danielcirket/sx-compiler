using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class WhileStatement : Statement
    {
        public BlockStatement Body { get; }
        public bool IsDoWhile { get; }
        public override SyntaxKind Kind => SyntaxKind.WhileStatement;

        public Expression Predicate { get; }

        public WhileStatement(ISourceFilePart span, bool isDoWhile, Expression predicate, BlockStatement body) : base(span)
        {
            Body = body;
            Predicate = predicate;
            IsDoWhile = isDoWhile;
        }
    }
}
