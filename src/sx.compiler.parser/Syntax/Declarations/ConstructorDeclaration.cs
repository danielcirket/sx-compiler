using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ConstructorDeclaration : Declaration
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.ConstructorDeclaration;
        public DeclarationVisibility Visibility { get; }
        public IEnumerable<ParameterDeclaration> Parameters { get; }

        public ConstructorDeclaration(ISourceFilePart span, DeclarationVisibility visibility, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, "constructor")
        {
            Visibility = visibility;
            Body = body;
            Parameters = parameters;
        }
    }
}
