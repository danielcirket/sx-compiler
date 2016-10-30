using System.Linq;

namespace Sxc.Lexer
{
    internal static class StringExtensions
    {
        public static bool IsKeyword(this string source, string[] keywords)
        {
            return keywords.Contains(source);
        }
    }
}
