using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundIdentifierExpression : BoundExpression
    {
        public BoundIdentifierExpression(IdentifierExpression expression)
            : base(expression)
        {

        }
    }
}
