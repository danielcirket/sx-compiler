using System;
using Sx.Lexer.Abstractions;

namespace Sx.Compiler.Lexer.Abstractions
{
    public class TokenMatch
    {
        public TokenType TokenType { get; private set; }
        public string Value { get; private set; }

        public TokenMatch(TokenType tokenType, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            TokenType = tokenType;
            Value = value;
        }
    }
}
