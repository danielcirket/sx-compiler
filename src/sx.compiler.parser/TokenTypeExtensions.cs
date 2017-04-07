using System.Collections.Generic;
using Sx.Lexer.Abstractions;

namespace Sx.Compiler.Parser
{
    internal static class TokenTypeExtensions
    {
        public static string GetValue(this TokenType source)
        {
            var lookup = new Dictionary<TokenType, string>
            {
                { TokenType.LineComment, "# or //" }, // #, //
                { TokenType.BlockComment, "/* or */" }, // /* */
                { TokenType.IntegerLiteral, string.Empty },
                { TokenType.StringLiteral, string.Empty },
                { TokenType.RealLiteral, string.Empty },
                { TokenType.Identifier, string.Empty },
                { TokenType.Keyword, string.Empty },
                { TokenType.LeftBracket, "{" }, // {
                { TokenType.RightBracket, "}" }, // }
                { TokenType.RightBrace, "]" }, // ]
                { TokenType.LeftBrace, "[" }, // [
                { TokenType.LeftParenthesis, "(" }, // (
                { TokenType.RightParenthesis, ")" }, // )
                { TokenType.GreaterThanOrEqual, ">=" }, // >=
                { TokenType.GreaterThan, ">" }, // >
                { TokenType.LessThan, "<" }, // <
                { TokenType.LessThanOrEqual, "<=" }, // <=
                { TokenType.PlusEqual, "+=" }, // +=
                { TokenType.PlusPlus, "++" }, // ++
                { TokenType.Plus, "+" }, // +
                { TokenType.MinusEqual, "-=" }, // -=
                { TokenType.MinusMinus, "--" }, // --
                { TokenType.Minus, "-" }, // -
                { TokenType.Assignment, "=" }, // =
                { TokenType.Not, "!" }, // !
                { TokenType.NotEqual,  "!=" },// !=
                { TokenType.Mul, "*" }, // *
                { TokenType.MulEqual, "*=" }, // *=
                { TokenType.Div, "/" }, // /
                { TokenType.DivEqual, "/=" }, // /=
                { TokenType.BooleanAnd, "&&" }, // &&
                { TokenType.BooleanOr, "||" }, // ||
                { TokenType.BitwiseAnd, "&" }, // &
                { TokenType.BitwiseOr, "|" }, // |
                { TokenType.BitwiseAndEqual, "&=" }, // &=
                { TokenType.BitwiseOrEqual, "|=" }, // |=
                { TokenType.ModEqual, "%=" }, // %=
                { TokenType.Mod, "%" }, // %
                { TokenType.BitwiseXorEqual, "^=" }, // ^=
                { TokenType.BitwiseXor, "^" }, // ^
                { TokenType.DoubleQuestion, "??" }, // ??
                { TokenType.Question, "?" }, // ?
                { TokenType.Equal, "==" }, // ==
                { TokenType.BitShiftLeft, "<<" }, // <<
                { TokenType.BitShiftRight, ">>" }, // >>
                { TokenType.Dot, "." },
                { TokenType.Comma, "," },
                { TokenType.Semicolon, ";" },
                { TokenType.Colon, ":" },
                { TokenType.Arrow, "->" }, // ->
                { TokenType.FatArrow, "=>" }, // =>
                { TokenType.CharLiteral, "'" }, // '
            };

            if (lookup.ContainsKey(source))
                return lookup[source];

            return string.Empty;
        }
    }
}
