using System.Collections.Generic;
using Sx.Compiler.Parser.BoundTree.Declarations;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.BoundTree
{
    public class BoundSourceDocument : BoundNode
    {
        public BoundSourceDocument(SourceDocument sourceDocument, IEnumerable<ImportStatement> imports, IEnumerable<BoundModuleDeclaration> boundModules, SymbolTable symbolTable)
            : base(sourceDocument)
        {

        }
    }
}
