using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class ElseStatement : Statement
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.ElseStatement;

        public ElseStatement(ISourceFilePart span, BlockStatement body) : base(span)
        {
            Body = body;
        }
    }
}
