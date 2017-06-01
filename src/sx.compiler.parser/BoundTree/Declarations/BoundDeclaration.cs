using Sx.Compiler.Parser.Syntax.Declarations;

namespace Sx.Compiler.Parser.BoundTree.Declarations
{
    public abstract class BoundDeclaration : BoundNode
    {
        public string Name { get; }

        protected BoundDeclaration(string name, Declaration node) : base(node)
        {
            Name = name;
        }
    }
}
