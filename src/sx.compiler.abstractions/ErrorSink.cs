using System.Collections;
using System.Collections.Generic;

namespace Sx.Compiler.Abstractions
{
    public class ErrorSink : IErrorSink
    {
        private List<IError> _errors;

        public IEnumerable<IError> Errors => _errors.AsReadOnly();
        public bool HasErrors => _errors.Count > 0;

        public void AddError(string message, ISourceFilePart sourceFilePart, Severity severity)
        {
            _errors.Add(new Error(message, sourceFilePart.GetLines(), severity, sourceFilePart));
        }
        public void Clear()
        {
            _errors.Clear();
        }
        public IEnumerator<IError> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        public ErrorSink()
        {
            _errors = new List<IError>();
        }
    }
}
