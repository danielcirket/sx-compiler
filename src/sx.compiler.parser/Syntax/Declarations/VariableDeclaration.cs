using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class VariableDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.VariableDeclaration;
        public TypeDeclaration Type { get; }
        public Expression Value { get; }

        public VariableDeclaration(ISourceFilePart span, string name, TypeDeclaration type, Expression value) : base(span, name)
        {
            Type = type;
            Value = value;
        }
        public VariableDeclaration(ISourceFilePart span, string name, TypeDeclaration type, Expression value, Scope scope) : base(span, name, scope)
        {
            Type = type;
            Value = value;
        }
        public VariableDeclaration(VariableDeclaration declaration, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Type, declaration.Value, scope)
        {

        }
        public VariableDeclaration(VariableDeclaration declaration, Expression value, Scope scope)
            : this(declaration.FilePart, declaration.Name, declaration.Type, value, scope)
        {

        }
    }
}
