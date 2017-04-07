using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public abstract class Statement : SyntaxNode
    {
        public override SyntaxCatagory Category => SyntaxCatagory.Statement;

        protected Statement(ISourceFilePart span) : base(span)
        {
        }
    }
}
