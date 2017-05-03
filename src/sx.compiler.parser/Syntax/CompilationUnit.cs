using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax
{
    public class CompilationUnit : SyntaxNode
    {
        public override SyntaxCatagory Category => SyntaxCatagory.CompilationUnit;
        public override SyntaxKind Kind => SyntaxKind.Invalid;
        public IEnumerable<SyntaxNode> Contents { get; }

        public CompilationUnit(ISourceFilePart filePart, IEnumerable<SyntaxNode> children) : base(filePart)
        {
            Contents = children ?? Enumerable.Empty<SyntaxNode>();
        }
    }
}
