using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class BreakStatement : EmptyStatement
    {
        public override SyntaxKind Kind => SyntaxKind.BreakStatement;

        public BreakStatement(ISourceFilePart span) : base(span)
        {
        }
    }
}
