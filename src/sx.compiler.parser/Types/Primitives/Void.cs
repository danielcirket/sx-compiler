using System;
using System.Collections.Generic;
using System.Text;

namespace Sx.Compiler.Parser.Types.Primitives
{
    public class Void : Type
    {
        public override string Name => "void";
        public override string FullName => Name;
    }
}
