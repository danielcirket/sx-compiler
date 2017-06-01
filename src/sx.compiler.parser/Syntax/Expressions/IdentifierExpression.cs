using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }
        public override SyntaxKind Kind => SyntaxKind.IdentifierExpression;

        public IdentifierExpression(ISourceFilePart span, string identifier) 
            : base(span)
        {
            Identifier = identifier;
        }
        public IdentifierExpression(ISourceFilePart span, string identifier, Scope scope) 
            : base(span, scope)
        {
            Identifier = identifier;
        }
        public IdentifierExpression(ISourceFilePart span, string identifier, Declaration binding, Scope scope) 
            : base(span, binding, scope)
        {
            Identifier = identifier;
        }
        public IdentifierExpression(IdentifierExpression expression, Declaration binding, Scope scope)
            : this(expression.FilePart, expression.Identifier, binding, scope)
        { 
        }
    }
}
