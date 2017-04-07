using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ConstructorDeclaration : Declaration
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.ConstructorDeclaration;
        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public ConstructorDeclaration(ISourceFilePart span, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, string.Empty)
        {
            Body = body;
            Parameters = parameters;
        }
    }
}
