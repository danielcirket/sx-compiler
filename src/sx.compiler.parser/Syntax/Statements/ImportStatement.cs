using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class ImportStatement : Statement
    {
        public IEnumerable<IdentifierExpression> Body { get; }
        public string Name => string.Join(".", Body.Select(b => b.Identifier));
        public override SyntaxKind Kind => SyntaxKind.ImportStatement;

        public ImportStatement(ISourceFilePart span, IEnumerable<IdentifierExpression> contents) : base(span)
        {
            Body = contents;
        }
    }
}
