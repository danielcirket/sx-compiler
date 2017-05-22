using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public abstract class Statement : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Statement;

        protected Statement(ISourceFilePart span) : base(span)
        {
        }
    }
}
