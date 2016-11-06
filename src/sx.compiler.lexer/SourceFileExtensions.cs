using System;
using System.Linq;
using Sx.Lexer.Abstractions;

namespace Sx.Lexer
{
    internal static class SourceFileExtensions
    {
        public static string[] GetLines(this ISourceFile source, int start, int end)
        {
            // TODO(Dan): Do this 'properly', i.e. remove the linq if it becomes an issue.
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (end < start)  
                throw new IndexOutOfRangeException("Cannot retrieve negative range");
          
            if (start < 1)
                throw new IndexOutOfRangeException($"{nameof(start)} must not be less than 1!");

            if (end > source.Lines.Length)
                throw new IndexOutOfRangeException("Cannot retrieve more lines than exist in file");

            return source.Lines.Skip(start - 1).Take(end - start).ToArray();
        }
    }
}
