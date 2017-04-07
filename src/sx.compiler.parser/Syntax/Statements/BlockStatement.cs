using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class BlockStatement : Statement
    {
        public IEnumerable<SyntaxNode> Contents { get; }
        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(ISourceFilePart span, IEnumerable<SyntaxNode> contents) : base(span)
        {
            Contents = contents;
        }
    }
}
