using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class ConstantExpression : Expression
    {
        public ConstantKind ConstentKind { get; }

        public override SyntaxKind Kind => SyntaxKind.ConstantExpression;
        public string Value { get; }

        public ConstantExpression(ISourceFilePart span, string value, ConstantKind kind)
            : base(span)
        {
            Value = value;
            ConstentKind = kind;
        }
    }
}
