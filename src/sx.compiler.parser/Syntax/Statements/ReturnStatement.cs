using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class ReturnStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.ReturnStatement;
        public Expression Value { get; }

        public ReturnStatement(ISourceFilePart span, Expression value) : base(span)
        {
            Value = value;
        }
    }
}
