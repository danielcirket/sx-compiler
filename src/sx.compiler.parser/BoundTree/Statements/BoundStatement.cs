using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public abstract class BoundStatement : BoundNode
    {
        protected BoundStatement(Statement statement)
            : base(statement)
        {
        }
    }
}
