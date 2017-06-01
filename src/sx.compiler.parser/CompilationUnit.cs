using System;
using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser
{
    public class CompilationUnit
    {
        public IEnumerable<SyntaxNode> Children { get; }
        public Scope Scope { get; }

        public CompilationUnit(IEnumerable<SyntaxNode> children)
        {
            Children = children ?? Enumerable.Empty<SyntaxNode>();
        }
        public CompilationUnit(CompilationUnit compilationUnit, IEnumerable<SyntaxNode> children, Scope scope)
            : this(children)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            Scope = scope;
        }
    }
}
