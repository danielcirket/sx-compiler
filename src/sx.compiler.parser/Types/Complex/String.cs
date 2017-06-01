using System;
using System.Collections.Generic;
using System.Text;

namespace Sx.Compiler.Parser.Types.Complex
{
    public class String : Type
    {
        public override string Name => "string";
        public override string FullName => Name;
    }
}
