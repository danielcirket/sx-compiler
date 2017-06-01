using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public abstract class Expression : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Expression;
        public Scope Scope { get; }
        public Declaration Binding { get; }

        protected Expression(ISourceFilePart span) : base(span)
        {
        }
        protected Expression(ISourceFilePart span, Scope scope) : base(span)
        {
            Scope = scope;
        }
        protected Expression(ISourceFilePart span, Declaration binding, Scope scope) : this(span, scope)
        {
            Binding = binding;
        }
    }
}
