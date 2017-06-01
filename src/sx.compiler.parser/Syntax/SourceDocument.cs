using System.Collections.Generic;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Syntax
{
    public class SourceDocument : SyntaxNode
    {
        public override SyntaxCategory Category => SyntaxCategory.Document;
        public IEnumerable<ImportStatement> Imports { get; }
        public IEnumerable<ModuleDeclaration> Modules { get; }
        public override SyntaxKind Kind => SyntaxKind.SourceDocument;
        public ISourceFile SourceCode { get; }
        public Scope Scope { get; }

        public SourceDocument(ISourceFilePart span, ISourceFile sourceCode, IEnumerable<ImportStatement> imports, IEnumerable<ModuleDeclaration> modules)
            : base(span)
        {
            SourceCode = sourceCode;
            Imports = imports;
            Modules = modules;
        }
        public SourceDocument(SourceDocument document, IEnumerable<ModuleDeclaration> modules, Scope scope)
            : this(document.FilePart, document.SourceCode, document.Imports, modules)
        {
            Scope = scope;
        }
    }
}
