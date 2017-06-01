using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundContinueStatement : BoundEmptyStatement
    {
        public BoundContinueStatement(ContinueStatement statement)
            : base(statement)
        {
        }
    }
}
