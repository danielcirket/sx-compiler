using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCatagory Category => SyntaxCatagory.Declaration;
        public string Name { get; }

        protected Declaration(ISourceFilePart span, string name) : base(span)
        {
            Name = name;
        }
    }
}
