using System;
using System.Collections.Generic;
using System.Text;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Semantics.Passes.Declarations
{
    public class DeclarationPass : SyntaxVisitor, ISemanticPass
    {
        private CompilationUnit _compilationUnit;
        private IErrorSink _errorSink;
        private readonly Stack<Scope> _scopes;

        public bool ShouldContinue => _errorSink.HasErrors;
        public void Run(IErrorSink errorSink, CompilationUnit compilationUnit)
        {
            if (compilationUnit == null)
                throw new ArgumentNullException(nameof(compilationUnit));

            _errorSink = errorSink;
            _compilationUnit = compilationUnit;
            _scopes.Push(new Scope());

            foreach (var item in compilationUnit.Asts)
                item.Accept(this);
        }

        protected override void VisitArithmetic(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitArrayAccess(ArrayAccessExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitAssignment(BinaryExpression expression)
        {
            expression.Right.Accept(this);
            expression.Left.Accept(this);
        }
        protected override void VisitBitwise(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitBlock(BlockStatement statement)
        {
            _scopes.Push(new Scope(_scopes.Peek()));

            _errorSink.AddError($"Use of undeclared identifier 'Test'", statement.FilePart, Severity.Error);

            foreach (var item in statement.Contents)
                item.Accept(this);

            _scopes.Pop();
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
            _scopes.Push(new Scope(_scopes.Peek()));

            foreach(var item in classDeclaration.Fields)
                item.Accept(this);

            foreach (var item in classDeclaration.Properties)
                item.Accept(this);

            foreach (var item in classDeclaration.Methods)
                item.Accept(this);

            foreach (var item in classDeclaration.Constructors)
                item.Accept(this);

            _scopes.Pop();
        }
        protected override void VisitCompilationUnit(CompilationUnit compilationUnit)
        {
            throw new NotImplementedException();
        }
        protected override void VisitConstant(ConstantExpression expression)
        {

        }
        protected override void VisitConstructor(ConstructorDeclaration constructorDeclaration)
        {
            _scopes.Push(new Scope(_scopes.Peek()));

            foreach (var item in constructorDeclaration.Parameters)
                item.Accept(this);

            constructorDeclaration.Body.Accept(this);

            _scopes.Pop();
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
            var currentScope = _scopes.Peek();

            if (!currentScope.Contains(expression.Identifier))
                _errorSink.AddError($"Use of undeclared identifier '{expression.Identifier}'", expression.FilePart, Severity.Error);
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
            throw new NotImplementedException();
        }
        protected override void VisitMethodCall(MethodCallExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitModuleDeclaration(ModuleDeclaration moduleDeclaration)
        {
            _scopes.Push(new Scope());

            foreach (var item in moduleDeclaration.Children)
                item.Accept(this);

            _scopes.Pop();
        }
        protected override void VisitNew(NewExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitParameter(ParameterDeclaration parameterDeclaration)
        {
            var currentScope = _scopes.Peek();

            if (currentScope.Contains(parameterDeclaration.Name))
                _errorSink.AddError($"Cannot redeclare identifier '{parameterDeclaration.Name}'", parameterDeclaration.FilePart, Severity.Error);
            else
                currentScope.Add(parameterDeclaration.Name, parameterDeclaration);
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
            throw new NotImplementedException();
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
            expression.Argument.Accept(this);
        }
        protected override void VisitVariable(VariableDeclaration variableDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override void VisitWhile(WhileStatement statement)
        {
            throw new NotImplementedException();
        }

        public DeclarationPass()
        {
            _scopes = new Stack<Scope>();
        }
    }
}
