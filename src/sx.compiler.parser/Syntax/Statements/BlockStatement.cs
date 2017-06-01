using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

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
        public BlockStatement(ISourceFilePart span, IEnumerable<SyntaxNode> contents, Scope scope) : base(span, scope)
        {
            Contents = contents;
        }
        public BlockStatement(BlockStatement statement, IEnumerable<SyntaxNode> contents, Scope scope)
            : this(statement.FilePart, contents, scope)
        {

        }
    }
}
