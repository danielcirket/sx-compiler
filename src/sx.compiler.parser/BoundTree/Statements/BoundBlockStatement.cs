using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundBlockStatement : BoundStatement
    {
        public BoundBlockStatement(BlockStatement statement)
            : base(statement)
        {
        }
    }
}
