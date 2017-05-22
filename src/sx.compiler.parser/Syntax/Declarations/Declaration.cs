using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Declaration;
        public string Name { get; }

        protected Declaration(ISourceFilePart span, string name) : base(span)
        {
            Name = name;
        }
    }
}
