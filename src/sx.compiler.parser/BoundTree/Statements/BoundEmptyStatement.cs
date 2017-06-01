using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree.Statements
{
    public class BoundEmptyStatement : BoundStatement
    {
        public BoundEmptyStatement(EmptyStatement statement)
            : base(statement)
        {
        }
    }
}
