using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundReferenceExpression : BoundExpression
    {
        public BoundReferenceExpression(ReferenceExpression expression)
            : base(expression)
        {
        }
    }
}
