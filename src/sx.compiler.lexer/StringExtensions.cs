using System;
using System.Linq;

namespace Sx.Lexer
{
    internal static class StringExtensions
    {
        public static bool IsKeyword(this string source, string[] keywords)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return keywords.Contains(source);
        }
    }
}
