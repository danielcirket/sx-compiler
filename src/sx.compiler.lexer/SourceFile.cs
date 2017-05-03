using System;
using Sx.Compiler.Abstractions;

namespace Sx.Lexer
{
    public class SourceFile : ISourceFile
    {
        private string _name;
        private string[] _lines;
        private string _source;

        public string Name => _name;
        public string Contents => _source;
        public string[] Lines => _lines;


        public SourceFile(string name, string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _name = name;
            _source = source;
            _lines = _source.Split(new [] { "\n", "\r\n" }, options: StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
