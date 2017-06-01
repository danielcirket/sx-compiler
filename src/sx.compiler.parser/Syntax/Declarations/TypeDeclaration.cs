using System;
using System.Linq;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class TypeDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.TypeDeclaration;
        public bool IsBuiltInType()
        {
            //new TokenMatch(TokenType.Keyword, "int"),
            //new TokenMatch(TokenType.Keyword, "string"),
            //new TokenMatch(TokenType.Keyword, "void"),
            //new TokenMatch(TokenType.Keyword, "float"),
            //new TokenMatch(TokenType.Keyword, "double"),
            //new TokenMatch(TokenType.Keyword, "decimal"),
            //new TokenMatch(TokenType.Keyword, "char"),
            return new[] { "int", "string", "void", "float", "double", "decimal", "char" }.Contains(Name);
        }

        public TypeDeclaration(ISourceFilePart span, string name) : base(span, name)
        {

        }
    }
}
