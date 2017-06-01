using System;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxCategory Category { get; }
        public abstract SyntaxKind Kind { get; }
        public ISourceFilePart FilePart { get; set; }

        public virtual void Accept(SyntaxVisitor visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }
        public virtual T Accept<T>(SyntaxVisitor<T> visitor)
        {
            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            return visitor.Visit(this);
        }

        protected SyntaxNode(ISourceFilePart filePart)
        {
            FilePart = filePart;
        }
    }
}
