using Sx.Compiler.Parser.Syntax.Expressions;

namespace Sx.Compiler.Parser.BoundTree.Expressions
{
    public class BoundLambdaExpression : BoundExpression
    {
        public BoundLambdaExpression(LambdaExpression expression)
            : base(expression)
        {
        }

        //public MethodDeclaration ToMethodDeclaration(string name, string type, DeclarationVisibility visibility) => new MethodDeclaration(FilePart, name, visibility, new TypeDeclaration(null, type), Enumerable.Empty<TypeDeclaration>(), Parameters, Body);
    }
}
