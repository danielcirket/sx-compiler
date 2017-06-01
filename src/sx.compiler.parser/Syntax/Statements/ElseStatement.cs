using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class ElseStatement : Statement
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.ElseStatement;

        public ElseStatement(ISourceFilePart span, BlockStatement body) : base(span)
        {
            Body = body;
        }
        public ElseStatement(ISourceFilePart span, BlockStatement body, Scope scope) : base(span, scope)
        {
            Body = body;
        }
        public ElseStatement(ElseStatement statement, BlockStatement body, Scope scope) 
            : this(statement.FilePart, body, scope)
        {
        }
    }
}
