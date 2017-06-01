using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundForStatement : BoundStatement
    {
        public BoundForStatement(ForStatement statement)
            : base(statement)
        {
        }
    }
}
