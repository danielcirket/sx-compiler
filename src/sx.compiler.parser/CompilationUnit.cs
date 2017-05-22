using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser
{
    public class CompilationUnit
    {
        public IEnumerable<SyntaxNode> Asts { get; }

        public CompilationUnit(IEnumerable<SyntaxNode> children)
        {
            Asts = children ?? Enumerable.Empty<SyntaxNode>();
        }
    }
}
