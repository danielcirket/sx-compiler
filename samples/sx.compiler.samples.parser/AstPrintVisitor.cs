using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Parser;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Samples.Parser
{
    public class AstPrintVisitor : SyntaxVisitor
    {
        private int _indent = 0;

        private void Print(string text)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
            {

                if (i == _indent - 1 && text != string.Empty)
                { 
                    result += "  \u2517";
                }
                else
                {
                    if (_indent > 0)
                    {

                    }

                    result += "  ";
                }
            }

            result += text;

            Console.WriteLine(result);
        }

        protected override void VisitArithmetic(BinaryExpression expression)
        {
            Print($" ARITHMATIC BINARY EXPRESION");

            _indent++;

            Visit(expression.Left);

            Print($" {expression.Operator}");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitArrayAccess(ArrayAccessExpression expression)
        {
            
        }
        protected override void VisitAssignment(BinaryExpression expression)
        {
            Print($" ASSIGNMENT BINARY EXPRESION");

            _indent++;

            Visit(expression.Left);

            Print(" = ");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitBitwise(BinaryExpression expression)
        {
            Print($" BITWISE BINARY EXPRESION");

            _indent++;

            Visit(expression.Left);

            Print("");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitBlock(BlockStatement statement)
        {
            Print($" BLOCK: {statement.Kind}");

             _indent++;

            foreach (var child in statement.Contents)
                Visit(child);

            _indent--;

            Print("");
        }
        protected override void VisitBreak(BreakStatement statement)
        {
            
        }
        protected override void VisitCase(CaseStatement statement)
        {
            
        }
        protected override void VisitClass(ClassDeclaration classDeclaration)
        {
            var genericTypes = classDeclaration.TypeDeclarations.Any()
                ? $"<{string.Join(", ", classDeclaration.TypeDeclarations.Select(x => x.Name))}>"
                : string.Empty;

            Print($" CLASS: {classDeclaration.Name}{genericTypes}");

            _indent++;

            foreach (var child in classDeclaration.Fields)
                Visit(child);

            foreach (var child in classDeclaration.Properties)
                Visit(child);

            foreach (var child in classDeclaration.Methods)
                Visit(child);

            foreach (var child in classDeclaration.Constructors)
                Visit(child);

            _indent--;

            Print("");
        }
        protected override void VisitConstant(ConstantExpression expression)
        {
            Print($" CONSTANT: {expression.Value} ({expression.ConstentKind})");
        }
        protected override void VisitConstructor(ConstructorDeclaration constructorDeclaration)
        {
            Print($" CONSTRUCTOR: {string.Join(", ", constructorDeclaration.Parameters.Select(x => x.Type.Name))}");

            _indent++;

            Visit(constructorDeclaration.Body);

            _indent--;

            Print("");
        }
        protected override void VisitContinue(ContinueStatement statement)
        {
            
        }
        protected override void VisitElse(ElseStatement statement)
        {
            
        }
        protected override void VisitEmpty(EmptyStatement statement)
        {
            
        }
        protected override void VisitField(FieldDeclaration fieldDeclaration)
        {
            Print($" FIELD: {fieldDeclaration.Name} {fieldDeclaration.Type.Name}");

            if (fieldDeclaration.DefaultValue != null)
            {
                _indent++;

                Visit(fieldDeclaration.DefaultValue);

                _indent--;
            }

            
        }
        protected override void VisitFor(ForStatement statement)
        {
            Print($" FOR LOOP:");

            _indent++;

            Print($" INITIALIZER:");
            _indent++;
            Visit(statement.Initialization);
            _indent--;

            Print($" CONDITION:");
            _indent++;
            Visit(statement.Condition);
            _indent--;

            Print($" INCREMENT:");
            _indent++;
            Visit(statement.Increment);
            _indent--;

            _indent--;

            Visit(statement.Body);

            _indent--;
        }
        protected override void VisitIdentifier(IdentifierExpression expression)
        {
            Print($" INDENTIFIER EXPRESION: {expression.Identifier}");
        }
        protected override void VisitIf(IfStatement statement)
        {
            
        }
        protected override void VisitImport(ImportStatement statement)
        {
            Print($"IMPORT STATEMENT: {statement.Name}");
        }
        protected override void VisitLambda(LambdaExpression expression)
        {
            Print($" LAMBDA EXPRESSION: ({string.Join(", ", expression.Parameters.Select(x => x.Type.Name))})");

            _indent++;

            Visit(expression.Body);

            _indent--;            
        }
        protected override void VisitLogical(BinaryExpression expression)
        {
            Print($" LOGICAL BINARY EXPRESION");

            _indent++;

            Visit(expression.Left);

            Print($" {expression.Operator}");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitMethod(MethodDeclaration methodDeclaration)
        {
            var genericTypes = methodDeclaration.TypeDeclarations.Any()
                ? $"<{string.Join(", ", methodDeclaration.TypeDeclarations.Select(x => x.Name))}>"
                : string.Empty;

            Print($" METHOD DECLARATION: ({methodDeclaration.Visibility}) {methodDeclaration.Name}{genericTypes}({string.Join(", ", methodDeclaration.Parameters.Select(x => string.Join(" ", x.Type.Name, x.Name)))}) => {methodDeclaration.ReturnType.Name}");

            _indent++;

            foreach (var child in methodDeclaration.Parameters)
                Visit(child);

            Visit(methodDeclaration.Body);

            _indent--;
        }
        protected override void VisitMethodCall(MethodCallExpression expression)
        {
            var temp = (expression.Reference is ReferenceExpression)
                ? string.Join(".", ((ReferenceExpression)(expression.Reference)).References.OfType<IdentifierExpression>().Select(x => x.Identifier))
                : string.Empty;
            
            Print($" METHOD CALL EXPRESION {temp}"); /*{(expression.Reference).Identifier}*/

            _indent++;

            foreach (var child in expression.Arguments)
                Visit(child);

            //Visit(expression.);

            //for (var i = 0; i < _indent; i++)
            //    Console.Write("    ");

            //Visit(expression.Right);

            _indent--;
        }
        protected override void VisitModuleDeclaration(ModuleDeclaration moduleDeclaration)
        {
            Print($"MODULE: {moduleDeclaration.Name}");

            _indent++;

            foreach (var child in moduleDeclaration.Children)
                Visit(child);

            _indent--;

            Print("");
        }
        protected override void VisitNew(NewExpression expression)
        {
            
        }
        protected override void VisitParameter(ParameterDeclaration parameterDeclaration)
        {
            
        }
        protected override void VisitProperty(PropertyDeclaration propertyDeclaration)
        {
            Print($" PROPERTY: {propertyDeclaration.Name} {propertyDeclaration.Type.Name}");

            _indent++;

            if (propertyDeclaration.GetMethod != null)
                Visit(propertyDeclaration.GetMethod);

            if (propertyDeclaration.SetMethod != null)
                Visit(propertyDeclaration.SetMethod);

            _indent--;
        }
        protected override void VisitReference(ReferenceExpression expression)
        {
            Print($" REFERENCE EXPRESION: ");

            _indent++;

            foreach (var reference in expression.References)
                Visit(reference);

            _indent--;
        }
        protected override void VisitSwitch(SwitchStatement statement)
        {
            
        }
        protected override void VisitUnary(UnaryExpression expression)
        {
            Print($" UNARY EXPRESSION: ");

            _indent++;

            Visit(expression.Argument);

            Print($" {expression.Operator}");

            _indent--;
        }
        protected override void VisitVariable(VariableDeclaration variableDeclaration)
        {
            Print($" VARIABLE DECLARATION: {variableDeclaration.Name}");

            _indent++;

             Visit(variableDeclaration.Value);

            _indent--;
        }
        protected override void VisitWhile(WhileStatement statement)
        {
            
        }
        protected override void VisitType(TypeDeclaration typeDeclaration)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
            {
                if (i == _indent - 1)
                    result += "  \u2517";
                else
                    result += "  ";
            }

            result += $" TYPE DECLARATION: {typeDeclaration.Name}";

            Console.WriteLine(result);
        }
        protected override void VisitReturn(ReturnStatement statement)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
            {
                if (i == _indent - 1)
                    result += "  \u2517";
                else
                    result += "  ";
            }

            result += $" RETURN STATEMENT:";

            Console.WriteLine(result);

            _indent++;

            result = string.Empty;

            Visit(statement.Value);

            _indent--;
        }

        protected override void VisitCompilationUnit(CompilationUnit compilationUnit)
        {
            foreach (var item in compilationUnit.Children)
                Visit(item);
        }

        public AstPrintVisitor()
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
        }
    }
}
