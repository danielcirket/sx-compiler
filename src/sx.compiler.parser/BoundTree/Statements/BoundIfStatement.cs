using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(IfStatement statement)
            : base(statement)
        {
        }
    }
}
