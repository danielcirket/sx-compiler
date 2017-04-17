using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Samples.Parser
{
    public class AstPrintVisitor : SyntaxVisitor
    {
        private int _indent = 0;

        protected override void VisitArithmetic(BinaryExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"ARITHMATIC BINARY EXPRESION";

            Console.WriteLine(result);

            _indent++;

            Visit(expression.Left);

            result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            Console.WriteLine(result += expression.Operator);

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitArrayAccess(ArrayAccessExpression expression)
        {
            
        }
        protected override void VisitAssignment(BinaryExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"ASSIGNMENT BINARY EXPRESION";

            Console.WriteLine(result);

            _indent++;

            Visit(expression.Left);

            result = string.Empty;
            for (var i = 0; i < _indent; i++)
                result += "    ";

            Console.WriteLine(result += " = ");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitBitwise(BinaryExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"BITWISE BINARY EXPRESION";

            Console.WriteLine(result);

            _indent++;

            Visit(expression.Left);

            for (var i = 0; i < _indent; i++)
                Console.Write("  ");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitBlock(BlockStatement statement)
            {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"BLOCK: {statement.Kind}";

            Console.WriteLine(result);

             _indent++;

            foreach (var child in statement.Contents)
                Visit(child);

            _indent--;

            result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"END BLOCK";

            Console.WriteLine(result);
        }
        protected override void VisitBreak(BreakStatement statement)
        {
            
        }
        protected override void VisitCase(CaseStatement statement)
        {
            
        }
        protected override void VisitClass(ClassDeclaration classDeclaration)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"CLASS: {classDeclaration.Name}";

            Console.WriteLine(result);

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

            result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"END CLASS";

            Console.WriteLine(result);
        }
        protected override void VisitConstant(ConstantExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"CONSTANT: {expression.Value} ({expression.ConstentKind})";

            Console.WriteLine(result);
        }
        protected override void VisitConstructor(ConstructorDeclaration constructorDeclaration)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"CONSTRUCTOR: {string.Join(", ", constructorDeclaration.Parameters.Select(x => x.Type.Name))}";

            Console.WriteLine(result);

            _indent++;

            Visit(constructorDeclaration.Body);

            _indent--;

            result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"END CONSTRUCTOR";

            Console.WriteLine(result);
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
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"FIELD: {fieldDeclaration.Name} {fieldDeclaration.Type.Name}";

            Console.WriteLine(result);

            if (fieldDeclaration.DefaultValue != null)
            {
                _indent++;

                Visit(fieldDeclaration.DefaultValue);

                _indent--;
            }

            
        }
        protected override void VisitFor(ForStatement statement)
        {
            
        }
        protected override void VisitIdentifier(IdentifierExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"INDENTIFIER EXPRESION: {expression.Identifier}";

            Console.WriteLine(result);
        }
        protected override void VisitIf(IfStatement statement)
        {
            
        }
        protected override void VisitImport(ImportStatement statement)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"IMPORT STATEMENT: {statement.Name}";

            Console.WriteLine(result);
        }
        protected override void VisitLambda(LambdaExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"LAMBDA EXPRESSION: ({string.Join(", ", expression.Parameters.Select(x => x.Type.Name))})";

            Console.WriteLine(result);

            _indent++;

            Visit(expression.Body);

            _indent--;            
        }
        protected override void VisitLogical(BinaryExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"LOGICAL BINARY EXPRESION";

            Console.WriteLine(result);

            _indent++;

            Visit(expression.Left);

            for (var i = 0; i < _indent; i++)
                Console.Write("    ");

            Visit(expression.Right);

            _indent--;
        }
        protected override void VisitMethod(MethodDeclaration methodDeclaration)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"METHOD DECLARATION: ({methodDeclaration.Visibility}) {methodDeclaration.Name}({string.Join(", ", methodDeclaration.Parameters.Select(x => x.Type.Name))}) => {methodDeclaration.ReturnType.Name}";

            Console.WriteLine(result);

            _indent++;

            foreach (var child in methodDeclaration.Parameters)
                Visit(child);

            Visit(methodDeclaration.Body);

            _indent--;
        }
        protected override void VisitMethodCall(MethodCallExpression expression)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"METHOD CALL EXPRESION {((IdentifierExpression)expression.Reference).Identifier}";

            Console.WriteLine(result);

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
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"MODULE: {moduleDeclaration.Name}";

            Console.WriteLine(result);

            _indent++;

            foreach (var child in moduleDeclaration.Classes)
                Visit(child);

            _indent--;

            result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"END MODULE";

            Console.WriteLine(result);
        }
        protected override void VisitNew(NewExpression expression)
        {
            
        }
        protected override void VisitParameter(ParameterDeclaration parameterDeclaration)
        {
            
        }
        protected override void VisitProperty(PropertyDeclaration propertyDeclaration)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"PROPERTY: {propertyDeclaration.Name} {propertyDeclaration.Type.Name}";

            Console.WriteLine(result);

            _indent++;

            if (propertyDeclaration.GetMethod != null)
                Visit(propertyDeclaration.GetMethod);

            if (propertyDeclaration.SetMethod != null)
                Visit(propertyDeclaration.SetMethod);

            _indent--;
        }
        protected override void VisitReference(ReferenceExpression expression)
        {
            
        }
        protected override void VisitSwitch(SwitchStatement statement)
        {
            
        }
        protected override void VisitUnary(UnaryExpression expression)
        {
            
        }
        protected override void VisitVariable(VariableDeclaration variableDeclaration)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"VARIABLE DECLARATION: {variableDeclaration.Name}";

            Console.WriteLine(result);

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
                result += "    ";

            result += $"TYPE DECLARATION: {typeDeclaration.Name}";

            Console.WriteLine(result);
        }
        protected override void VisitReturn(ReturnStatement statement)
        {
            var result = string.Empty;

            for (var i = 0; i < _indent; i++)
                result += "    ";

            result += $"RETURN STATEMENT:";

            Console.WriteLine(result);

            _indent++;

            result = string.Empty;

            Visit(statement.Value);

            _indent--;
        }

        public AstPrintVisitor()
        {

        }
    }
}
