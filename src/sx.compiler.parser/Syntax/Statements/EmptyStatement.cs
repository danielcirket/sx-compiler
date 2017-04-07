using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class EmptyStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.EmptyStatement;

        public EmptyStatement(ISourceFilePart span) : base(span)
        {
        }
    }
}
