using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Statements
{
    public class SwitchStatement : Statement
    {
        public IEnumerable<CaseStatement> Cases { get; }
        public override SyntaxKind Kind => SyntaxKind.SwitchStatement;

        public SwitchStatement(ISourceFilePart span, IEnumerable<CaseStatement> cases) : base(span)
        {
            Cases = cases;
        }
    }
}
