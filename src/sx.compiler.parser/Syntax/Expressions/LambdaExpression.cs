using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Syntax.Expressions
{
    public class LambdaExpression : Expression
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.LambdaExpression;
        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public LambdaExpression(ISourceFilePart span, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span)
        {
            Parameters = parameters;
            Body = body;
        }

        public MethodDeclaration ToMethodDeclaration(string name, string type, DeclarationVisibility visibility) => new MethodDeclaration(FilePart, name, visibility, new TypeDeclaration(null, type), Parameters, Body);
    }
}
