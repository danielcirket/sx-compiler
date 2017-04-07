using System.Collections.Generic;
using Sx.Compiler.Abstractions;

namespace Sx.Compiler.Parser.Syntax.Declarations
{
    public class ModuleDeclaration : Declaration
    {
        public IEnumerable<ClassDeclaration> Classes { get; }
        public override SyntaxKind Kind => SyntaxKind.ModuleDeclaration;

        public ModuleDeclaration(ISourceFilePart span, string name, IEnumerable<ClassDeclaration> children) : base(span, name)
        {
            Classes = children;
        }
    }
}
