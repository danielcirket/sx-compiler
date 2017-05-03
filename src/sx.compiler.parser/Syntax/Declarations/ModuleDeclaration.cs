using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ModuleDeclaration : Declaration
    {
        public IEnumerable<Declaration> Children { get; }
        public override SyntaxKind Kind => SyntaxKind.ModuleDeclaration;

        public ModuleDeclaration(ISourceFilePart span, string name, IEnumerable<Declaration> children) : base(span, name)
        {
            Children = children;
        }
    }
}
