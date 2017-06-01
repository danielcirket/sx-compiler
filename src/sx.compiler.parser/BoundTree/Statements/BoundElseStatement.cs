using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundElseStatement : BoundStatement
    {
        public BoundElseStatement(ElseStatement statement)
            : base(statement)
        {
        }
    }
}
