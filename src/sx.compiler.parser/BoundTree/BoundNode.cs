using System;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser.BoundTree
{
    public abstract class BoundNode
    {
        public SyntaxNode SyntaxNode { get; }
        public Scope Scope { get; }

        public virtual void Accept(BoundTreeVisitor visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }
    }
}
