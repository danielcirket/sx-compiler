using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundBreakStatement : BoundEmptyStatement
    {
        public BoundBreakStatement(BreakStatement statement)
            : base(statement)
        {
        }
    }
}
