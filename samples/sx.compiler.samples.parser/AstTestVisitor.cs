using System;
using System.Linq;
using System.Text;
using Sx.Compiler.Parser;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Samples.Parser
{
    internal class AstTestVisitor : SyntaxVisitor
    {
        private StringBuilder _sb = new StringBuilder();
        private string _currentReturnType = string.Empty;

        public string Content => _sb.ToString();

        protected override void VisitArithmetic(BinaryExpression expression)
        {
            _sb.Append($"{_currentReturnType}.{expression.Operator.ToString().ToLower()} ");

            Visit(expression.Left);
            Visit(expression.Right);
            //throw new NotImplementedException();
        }
        protected override void VisitArrayAccess(ArrayAccessExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitAssignment(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitBitwise(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitBlock(BlockStatement statement)
        {
            _sb.Append("(");

            foreach (var child in statement.Contents)
                Visit(child);
             
            _sb.Append(")");
        }
        protected override void VisitBreak(BreakStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitCase(CaseStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitClass(ClassDeclaration classDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitConstant(ConstantExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitConstructor(ConstructorDeclaration constructorDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitContinue(ContinueStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitElse(ElseStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitEmpty(EmptyStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitField(FieldDeclaration fieldDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitFor(ForStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitIdentifier(IdentifierExpression expression)
        {
            _sb.Append($"(get_local {expression.Identifier})");

            //throw new NotImplementedException();
        }
        protected override void VisitIf(IfStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitImport(ImportStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitLambda(LambdaExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitLogical(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitMethod(MethodDeclaration methodDeclaration)
        {
            _sb.Append($"  (func ${methodDeclaration.Name} (param");

            foreach (var child in methodDeclaration.Parameters)
            {
                Visit(child);
            }

            var type = methodDeclaration.ReturnType.Name == "int" 
                ? "i32" 
                : methodDeclaration.ReturnType.Name;

            _sb.AppendLine($") (result {type})");

            _currentReturnType = type;

            _sb.Append("    ");

            Visit(methodDeclaration.Body);

            _sb.AppendLine();

            _currentReturnType = string.Empty;

            if (methodDeclaration.Visibility == DeclarationVisibility.Public)                
                _sb.AppendLine($"  (export \"{methodDeclaration.Name}\" (func ${methodDeclaration.Name}))");
        }
        protected override void VisitMethodCall(MethodCallExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitModuleDeclaration(ModuleDeclaration moduleDeclaration)
        {
            _sb.AppendLine("(module");

            foreach (var child in moduleDeclaration.Children)
                Visit(child);

            _sb.AppendLine(")");
        }
        protected override void VisitNew(NewExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitParameter(ParameterDeclaration parameterDeclaration)
        {
            var type = parameterDeclaration.Type.Name == "int"
                ? "i32"
                : parameterDeclaration.Type.Name;

            _sb.Append($" {type} ");
        }
        protected override void VisitProperty(PropertyDeclaration propertyDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitReference(ReferenceExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitReturn(ReturnStatement statement)
        {
            // TODO(Dan): What to do with this?
            Visit(statement.Value);

            //throw new NotImplementedException();
        }
        protected override void VisitSwitch(SwitchStatement statement)
        {
            throw new NotImplementedException();
        }
        protected override void VisitType(TypeDeclaration typeDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitUnary(UnaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitVariable(VariableDeclaration variableDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitWhile(WhileStatement statement)
        {
            throw new NotImplementedException();
        }

        private static string Operator(BinaryOperator source)
        {
            switch (source)
            {
                case BinaryOperator.Assign:
                    return "=";
                case BinaryOperator.AddAssign:
                    return "+=";
                case BinaryOperator.SubAssign:
                    return "-=";
                case BinaryOperator.MulAssign:
                    return "*=";
                case BinaryOperator.DivAssign:
                    return "/=";
                case BinaryOperator.ModAssign:
                    return "%=";
                case BinaryOperator.AndAssign:
                    return "&=";
                case BinaryOperator.XorAssign:
                    return "^=";
                case BinaryOperator.OrAssign:
                    return "|=";
                case BinaryOperator.LogicalOr:
                    return "|";
                case BinaryOperator.LogicalAnd:
                    return "&";
                case BinaryOperator.Equal:
                    return "==";
                case BinaryOperator.NotEqual:
                    return "!=";
                case BinaryOperator.GreaterThan:
                    return ">";
                case BinaryOperator.LessThan:
                    return "<";
                case BinaryOperator.GreaterThanOrEqual:
                    return ">=";
                case BinaryOperator.LessThanOrEqual:
                    return "<=";
                case BinaryOperator.BitwiseAnd:
                    return "&";
                case BinaryOperator.BitwiseOr:
                    return "|";
                case BinaryOperator.BitwiseXor:
                    return "^";
                case BinaryOperator.LeftShift:
                    return "<<";
                case BinaryOperator.RightShift:
                    return ">>";
                case BinaryOperator.Add:
                    return "+";
                case BinaryOperator.Sub:
                    return "-";
                case BinaryOperator.Mul:
                    return "*";
                case BinaryOperator.Div:
                    return "/";
                case BinaryOperator.Mod:
                    return "%";

                default:
                    throw new ArgumentException(nameof(source));
            }
        }

        protected override void VisitCompilationUnit(CompilationUnit compilationUnit)
        {
            foreach (var item in compilationUnit.Children)
                Visit(item);
        }
    }
}
