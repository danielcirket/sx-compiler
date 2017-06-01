using System;
using System.Collections.Generic;
using System.Text;

namespace Sx.Compiler.Parser.Types.Complex
{
    public class Array<T> : Type
    {
        public override string Name => $"Array<{typeof(T).Name}>";
        public override string FullName => Name;
    }
}
