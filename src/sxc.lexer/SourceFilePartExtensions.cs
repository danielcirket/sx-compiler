using System.Linq;
using Sxc.Lexer.Abstractions;

namespace Sxc.Lexer
{
    internal static class SourceFilePartExtensions
    {
        public static string[] GetLines(this ISourceFilePart source)
        {
            // TODO(Dan): Do this 'properly', i.e. remove the linq if it becomes an issue.
            return source.Lines.Skip(source.Start.Index - 1).Take(source.End.Index - source.Start.Index).ToArray();
        }
    }
}
