using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundImportStatement : BoundStatement
    {
        public BoundImportStatement(ImportStatement statement)
            : base(statement)
        {
        }
    }
}
