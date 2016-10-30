using Sxc.Lexer.Abstractions;

namespace Sxc.Lexer
{
    internal class Error : IError
    {
        public string[] Lines { get; }
        public string Message { get; }
        public Severity Severity { get; }
        public ISourceFilePart FilePart { get; }

        public Error(string message, string[] lines, Severity severity, ISourceFilePart part)
        {
            Message = message;
            Lines = lines;
            Severity = severity;
            FilePart = part;
        }
    }
}
