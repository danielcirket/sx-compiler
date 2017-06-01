using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundWhileStatement : BoundStatement
    {
        public BoundWhileStatement(WhileStatement statement)
            : base(statement)
        {
        }
    }
}
