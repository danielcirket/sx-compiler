using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundReturnStatement : BoundStatement
    {
        public BoundReturnStatement(ReturnStatement statement)
            : base(statement)
        {
        }
    }
}
