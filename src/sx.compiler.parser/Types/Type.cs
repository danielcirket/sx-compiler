using System;
using System.Collections.Generic;
using System.Text;

namespace Sx.Compiler.Parser.Types
{
    public abstract class Type
    {
        /// <summary>
        /// Type name, e.g. Int32
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Fully qualified type name, e.g. Some.Module.Int32
        /// </summary>
        public abstract string FullName { get; }
    }
}
