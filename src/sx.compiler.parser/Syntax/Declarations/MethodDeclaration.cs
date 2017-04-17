using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class MethodDeclaration : Declaration
    {
        public BlockStatement Body { get; }
        public override SyntaxKind Kind => SyntaxKind.MethodDeclaration;
        public DeclarationVisibility Visibility { get; }
        public IEnumerable<ParameterDeclaration> Parameters { get; }
        public TypeDeclaration ReturnType { get; }

        public MethodDeclaration(ISourceFilePart span, string name, DeclarationVisibility visibility, TypeDeclaration returnType, IEnumerable<ParameterDeclaration> parameters, BlockStatement body) : base(span, name)
        {
            Visibility = visibility;
            ReturnType = returnType;
            Parameters = parameters;
            Body = body;
        }
    }
}
