using System;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser.BoundTree
{
    public abstract class BoundNode
    {
        public SyntaxNode SyntaxNode { get; }

        public virtual void Accept(BoundTreeVisitor visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        public BoundNode(SyntaxNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            SyntaxNode = node;
        }
    }
}
