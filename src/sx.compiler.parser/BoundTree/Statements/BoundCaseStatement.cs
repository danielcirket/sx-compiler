using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundCaseStatement : BoundStatement
    {
        public BoundCaseStatement(CaseStatement statement)
            : base(statement)
        {
        }
    }
}
