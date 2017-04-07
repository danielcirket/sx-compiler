using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax
{
    public class SourceDocument : SyntaxNode
    {
        public override SyntaxCatagory Category => SyntaxCatagory.Document;
        public IEnumerable<SyntaxNode> Children { get; }
        public override SyntaxKind Kind => SyntaxKind.SourceDocument;
        public ISourceFile SourceCode { get; }

        public SourceDocument(ISourceFilePart span, ISourceFile sourceCode, IEnumerable<SyntaxNode> children)
            : base(span)
        {
            SourceCode = sourceCode;
            Children = children;
        }
    }
}
