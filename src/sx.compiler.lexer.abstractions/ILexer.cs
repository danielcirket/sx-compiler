using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Lexer.Abstractions
{
    public interface ILexer
    {
        IErrorSink ErrorSink { get; }
        IEnumerable<IToken> Tokenize(ISourceFile sourceFile);
        IEnumerable<IToken> Tokenize(string sourceFileContent);
    }
}
