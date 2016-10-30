using System;
using Sxc.Lexer.Abstractions;

namespace Sxc.Lexer
{
    internal class SourceFile : ISourceFile
    {
        private string[] _lines;
        private string _source;

        public string Contents => _source;
        public string[] Lines => _lines;

        public SourceFile(string source)
        {
            _source = source;
            _lines = _source.Split(new [] { Environment.NewLine }, options: StringSplitOptions.None);
        }
    }
}
