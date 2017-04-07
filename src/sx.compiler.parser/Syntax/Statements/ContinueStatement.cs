using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class ContinueStatement : EmptyStatement
    {
        public override SyntaxKind Kind => SyntaxKind.ContinueStatement;

        public ContinueStatement(ISourceFilePart span) : base(span)
        {
        }
    }
}
