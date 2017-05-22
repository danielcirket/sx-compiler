using System;
using System.Collections.Generic;
using System.Text;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Semantics.Passes.TypeInference
{
    public class TypeInferencePass : SyntaxVisitor, ISemanticPass
    {
        public bool ShouldContinue => throw new NotImplementedException();

        public void Run(IErrorSink errorSink, CompilationUnit compilationUnit)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        protected override void VisitBitwise(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitBlock(BlockStatement statement)
        {
            throw new NotImplementedException();
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
        protected override void VisitCompilationUnit(CompilationUnit compilationUnit)
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
        protected override void VisitNew(NewExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override void VisitParameter(ParameterDeclaration parameterDeclaration)
        {
            throw new NotImplementedException();
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
    }
}
