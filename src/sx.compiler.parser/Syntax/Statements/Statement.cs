using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public abstract class Statement : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Statement;
        public Scope Scope { get; }

        protected Statement(ISourceFilePart span) : base(span)
        {
        }
        protected Statement(ISourceFilePart span, Scope scope) : base(span)
        {
            Scope = scope;
        }
    }
}
