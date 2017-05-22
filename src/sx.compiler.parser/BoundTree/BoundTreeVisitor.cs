using Sx.Compiler.Parser.BoundTree.Declarations;
using Sx.Compiler.Parser.BoundTree.Expressions;
using Sx.Compiler.Parser.BoundTree.Statements;
using Sx.Compiler.Parser.Syntax;

namespace Sx.Compiler.Parser.BoundTree
{
    public abstract class BoundTreeVisitor
    {
        public void Visit(BoundNode node)
        {
            switch (node.SyntaxNode.Category)
            {
                case SyntaxCategory.Document:
                    VisitDocument(node as BoundSourceDocument);
                    break;

                case SyntaxCategory.Expression:
                    VisitExpression(node as BoundExpression);
                    break;

                case SyntaxCategory.Statement:
                    VisitStatement(node as BoundStatement);
                    break;

                case SyntaxCategory.Declaration:
                    VisitDeclaration(node as BoundDeclaration);
                    break;
            }
        }

        protected void VisitExpression(BoundExpression expression)
        {
            switch (expression.Kind)
            {
                case SyntaxKind.ArrayAccessExpression:
                    VisitArrayAccess(expression as BoundArrayAccessExpression);
                    break;

                case SyntaxKind.BinaryExpression:
                    VisitBinary(expression as BoundBinaryExpression);
                    break;

                case SyntaxKind.ConstantExpression:
                    VisitConstant(expression as BoundConstantExpression);
                    break;

                case SyntaxKind.IdentifierExpression:
                    VisitIdentifier(expression as BoundIdentifierExpression);
                    break;

                case SyntaxKind.LambdaExpression:
                    VisitLambda(expression as BoundLambdaExpression);
                    break;

                case SyntaxKind.MethodCallExpression:
                    VisitMethodCall(expression as BoundMethodCallExpression);
                    break;

                case SyntaxKind.NewExpression:
                    VisitNew(expression as BoundNewExpression);
                    break;

                case SyntaxKind.ReferenceExpression:
                    VisitReference(expression as BoundReferenceExpression);
                    break;

                case SyntaxKind.UnaryExpression:
                    VisitUnary(expression as BoundUnaryExpression);
                    break;
            }
        }
        protected void VisitBinary(BoundBinaryExpression expression)
        {
            switch (expression.Operator)
            {
                case BinaryOperator.Add:
                case BinaryOperator.Div:
                case BinaryOperator.Mod:
                case BinaryOperator.Sub:
                case BinaryOperator.Mul:
                    VisitArithmetic(expression);
                    break;

                case BinaryOperator.Assign:
                case BinaryOperator.AddAssign:
                case BinaryOperator.AndAssign:
                case BinaryOperator.DivAssign:
                case BinaryOperator.ModAssign:
                case BinaryOperator.MulAssign:
                case BinaryOperator.OrAssign:
                case BinaryOperator.SubAssign:
                case BinaryOperator.XorAssign:
                    VisitAssignment(expression);
                    break;

                case BinaryOperator.Equal:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.LogicalAnd:
                case BinaryOperator.LogicalOr:
                case BinaryOperator.NotEqual:
                    VisitLogical(expression);
                    break;

                case BinaryOperator.BitwiseAnd:
                case BinaryOperator.BitwiseOr:
                case BinaryOperator.BitwiseXor:
                    VisitBitwise(expression);
                    break;
            }
        }
        protected void VisitStatement(BoundStatement statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.ImportStatement:
                    VisitImport(statement as BoundImportStatement);
                    break;

                case SyntaxKind.BlockStatement:
                    VisitBlock(statement as BoundBlockStatement);
                    break;

                case SyntaxKind.BreakStatement:
                    VisitBreak(statement as BoundBreakStatement);
                    break;

                case SyntaxKind.CaseStatement:
                    VisitCase(statement as BoundCaseStatement);
                    break;

                case SyntaxKind.ContinueStatement:
                    VisitContinue(statement as BoundContinueStatement);
                    break;

                case SyntaxKind.ElseStatement:
                    VisitElse(statement as BoundElseStatement);
                    break;

                case SyntaxKind.EmptyStatement:
                    VisitEmpty(statement as BoundEmptyStatement);
                    break;

                case SyntaxKind.ForStatement:
                    VisitFor(statement as BoundForStatement);
                    break;

                case SyntaxKind.IfStatement:
                    VisitIf(statement as BoundIfStatement);
                    break;

                case SyntaxKind.SwitchStatement:
                    VisitSwitch(statement as BoundSwitchStatement);
                    break;

                case SyntaxKind.WhileStatement:
                    VisitWhile(statement as BoundWhileStatement);
                    break;

                case SyntaxKind.ReturnStatement:
                    VisitReturn(statement as BoundReturnStatement);
                    break;
            }
        }
        protected void VisitDeclaration(BoundDeclaration node)
        {
            switch (node.Kind)
            {
                case SyntaxKind.ModuleDeclaration:
                    VisitModuleDeclaration(node as BoundModuleDeclaration);
                    break;

                case SyntaxKind.ClassDeclaration:
                    VisitClass(node as BoundClassDeclaration);
                    break;

                case SyntaxKind.FieldDeclaration:
                    VisitField(node as BoundFieldDeclaration);
                    break;

                case SyntaxKind.ConstructorDeclaration:
                    VisitConstructor(node as BoundConstructorDeclaration);
                    break;

                case SyntaxKind.PropertyDeclaration:
                    VisitProperty(node as BoundPropertyDeclaration);
                    break;

                case SyntaxKind.ParameterDeclaration:
                    VisitParameter(node as BoundParameterDeclaration);
                    break;

                case SyntaxKind.MethodDeclaration:
                    VisitMethod(node as BoundMethodDeclaration);
                    break;

                case SyntaxKind.VariableDeclaration:
                    VisitVariable(node as BoundVariableDeclaration);
                    break;

                case SyntaxKind.TypeDeclaration:
                    VisitType(node as BoundTypeDeclaration);
                    break;
            }
        }
        protected void VisitDocument(BoundSourceDocument sourceDocument)
        {
            foreach (var node in sourceDocument.Children)
            {
                Visit(node);
            }
        }

        protected abstract void VisitArithmetic(BoundBinaryExpression expression);
        protected abstract void VisitArrayAccess(BoundArrayAccessExpression expression);
        protected abstract void VisitAssignment(BoundBinaryExpression expression);
        protected abstract void VisitBitwise(BoundBinaryExpression expression);
        protected abstract void VisitBlock(BoundBlockStatement statement);
        protected abstract void VisitImport(BoundImportStatement statement);
        protected abstract void VisitBreak(BoundBreakStatement statement);
        protected abstract void VisitCase(BoundCaseStatement statement);
        protected abstract void VisitClass(BoundClassDeclaration classDeclaration);
        protected abstract void VisitConstant(BoundConstantExpression expression);
        protected abstract void VisitConstructor(BoundConstructorDeclaration constructorDeclaration);
        protected abstract void VisitContinue(BoundContinueStatement statement);
        protected abstract void VisitElse(BoundElseStatement statement);
        protected abstract void VisitEmpty(BoundEmptyStatement statement);
        protected abstract void VisitField(BoundFieldDeclaration fieldDeclaration);
        protected abstract void VisitFor(BoundForStatement statement);
        protected abstract void VisitIdentifier(BoundIdentifierExpression expression);
        protected abstract void VisitIf(BoundIfStatement statement);
        protected abstract void VisitLambda(BoundLambdaExpression expression);
        protected abstract void VisitLogical(BoundBinaryExpression expression);
        protected abstract void VisitMethod(BoundMethodDeclaration methodDeclaration);
        protected abstract void VisitType(BoundTypeDeclaration typeDeclaration);
        protected abstract void VisitMethodCall(BoundMethodCallExpression expression);
        protected abstract void VisitNew(BoundNewExpression expression);
        protected abstract void VisitModuleDeclaration(BoundModuleDeclaration moduleDeclaration);
        protected abstract void VisitParameter(BoundParameterDeclaration parameterDeclaration);
        protected abstract void VisitProperty(BoundPropertyDeclaration propertyDeclaration);
        protected abstract void VisitReference(BoundReferenceExpression expression);
        protected abstract void VisitSwitch(BoundSwitchStatement statement);
        protected abstract void VisitUnary(BoundUnaryExpression expression);
        protected abstract void VisitVariable(BoundVariableDeclaration variableDeclaration);
        protected abstract void VisitWhile(BoundWhileStatement statement);
        protected abstract void VisitReturn(BoundReturnStatement statement);
        protected abstract void VisitCompilationUnit(CompilationUnit compilationUnit);
    }
}
