using System.Collections.Generic;

namespace Sxc.Lexer.Abstractions
{
    public interface IErrorSink : IEnumerable<IError>
    {
        IEnumerable<IError> Errors { get; }
        bool HasErrors { get; }

        void AddError(string message, ISourceFilePart sourceFilePart, Severity severity);
        void Clear();
    }
}
