using System.Collections.Generic;

namespace Sx.Compiler.Abstractions
{
    public interface IErrorSink : IEnumerable<IError>
    {
        IEnumerable<IError> Errors { get; }
        bool HasErrors { get; }

        void AddError(string message, ISourceFilePart sourceFilePart, Severity severity);
        void Clear();
    }
}
