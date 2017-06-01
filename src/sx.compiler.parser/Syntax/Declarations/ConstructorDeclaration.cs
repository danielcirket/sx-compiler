using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
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
        public ConstructorDeclaration(ISourceFilePart span, DeclarationVisibility visibility, IEnumerable<ParameterDeclaration> parameters, BlockStatement body, Scope scope) 
            : base(span, "constructor", scope)
        {
            Visibility = visibility;
            Body = body;
            Parameters = parameters;
        }
        public ConstructorDeclaration(ConstructorDeclaration declaration, Scope scope)
            : this(declaration.FilePart, declaration.Visibility, declaration.Parameters, declaration.Body, scope)
        {

        }
        public ConstructorDeclaration(ConstructorDeclaration declaration, IEnumerable<ParameterDeclaration> parameters, BlockStatement body, Scope scope)
            : this(declaration.FilePart, declaration.Visibility, declaration.Parameters, body, scope)
        {

        }
    }
}
