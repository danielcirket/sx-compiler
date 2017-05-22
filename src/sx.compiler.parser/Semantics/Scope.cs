using System;
using System.Collections.Generic;
using System.Text;
using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.Semantics
{
    public class Scope
    {
        private readonly Dictionary<string, Declaration> _declarations;
        private readonly Scope _parent;

        public bool Contains(string name)
        {
            if (_declarations.ContainsKey(name))
                return true;

            if (_parent != null)
                return _parent.Contains(name);

            return false;
        }
        public void Add(string name, Declaration node) => _declarations.Add(name, node);

        public Scope()
        {
            _declarations = new Dictionary<string, Declaration>();
        }
        public Scope(Scope parent) : this()
        {
            _parent = parent;
        }
    }
}
