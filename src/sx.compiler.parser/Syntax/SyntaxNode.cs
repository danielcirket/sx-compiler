using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxCatagory Category { get; }
        public abstract SyntaxKind Kind { get; }
        public ISourceFilePart FilePart { get; set; }

        protected SyntaxNode(ISourceFilePart filePart)
        {
            FilePart = filePart;
        }
    }
}
