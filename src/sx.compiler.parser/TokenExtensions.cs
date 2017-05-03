using System;
using System.Linq;
using Sx.Lexer.Abstractions;

namespace Sx.Compiler.Parser
{
    internal static class TokenExtensions
    {
        internal static bool IsTypeKeyword(this IToken source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //new TokenMatch(TokenType.Keyword, "int"),
            //    new TokenMatch(TokenType.Keyword, "string"),
            //    new TokenMatch(TokenType.Keyword, "void"),
            //    new TokenMatch(TokenType.Keyword, "float"),
            //    new TokenMatch(TokenType.Keyword, "double"),
            //    new TokenMatch(TokenType.Keyword, "decimal"),
            //    new TokenMatch(TokenType.Keyword, "char"),

            return new[] { "int", "string", "void", "float", "double", "decimal", "char" }.Contains(source.Value);
        }
    }
}
