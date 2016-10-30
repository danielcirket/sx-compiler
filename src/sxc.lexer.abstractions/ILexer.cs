using System.Collections.Generic;

namespace Sxc.Lexer.Abstractions
{
    public interface ILexer
    {
        IErrorSink ErrorSink { get; }
        IEnumerable<IToken> Tokenize(ISourceFile sourceFile);
        IEnumerable<IToken> Tokenize(string sourceFileContent);
    }
}
