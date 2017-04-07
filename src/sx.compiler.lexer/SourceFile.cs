using System;
using Sx.Compiler.Abstractions;

namespace Sx.Lexer
{
    internal class SourceFile : ISourceFile
    {
        private string[] _lines;
        private string _source;

        public string Contents => _source;
        public string[] Lines => _lines;

        public SourceFile(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _source = source;
            _lines = _source.Split(new [] { Environment.NewLine }, options: StringSplitOptions.None);
        }
    }
}
