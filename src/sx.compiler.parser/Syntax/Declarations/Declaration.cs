using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Declaration;
        public string Name { get; }
        public Scope Scope { get; }

        protected Declaration(ISourceFilePart span, string name) : base(span)
        {
            Name = name;
        }
        protected Declaration(ISourceFilePart span, string name, Scope scope) : this(span, name)
        {
            Scope = scope;
        }
    }
}
