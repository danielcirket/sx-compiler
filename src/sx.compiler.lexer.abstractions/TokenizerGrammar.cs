using System.Collections.Generic;

namespace Sx.Compiler.Lexer.Abstractions
{
    public class TokenizerGrammar
    {
        public List<TokenMatch> Keywords { get; set; }
        public List<TokenMatch> SpecialCharacters { get; set; }
    }
}
