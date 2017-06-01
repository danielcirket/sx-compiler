using System;
using System.Collections.Generic;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Compiler.Parser.Syntax.Statements;

namespace Sx.Compiler.Parser.Semantics.Passes.Declarations
{
    /// <summary>
    /// This goes through the compilation unit's ASTs, then proceeds to go through each module declaration
    /// looking only at the top level method declarations and class declarations.
    /// 
    /// For class declarations we also get the information for the fields, properties, methods and constructors,
    /// however we do not visit past the delcaration itself.
    /// 
    /// This allows us to know the structure of the module and classes before using their symbols in the child scopes
    /// so things don't need to be forward declared.
    /// 
    /// Obviously this is only a temporary pass as it would be better to fill out this info while we process the syntax
    /// and might be bound at that time, but for simplicity we will do this for now.
    /// 
    /// We want to keep this immutable rather than tracking state everywhere!
    /// </summary>
    public class DeclarationPass : SyntaxVisitor<SyntaxNode>, ISemanticPass
    {
        private CompilationUnit _compilationUnit;
        private IErrorSink _errorSink;
        private Scope _rootScope;
        private Stack<Scope> _scopes;
        private SourceDocument _currentDocument;
        private ModuleDeclaration _currentModule;

        public bool ShouldContinue => !_errorSink.HasErrors;
        public void Run(IErrorSink errorSink, ref CompilationUnit compilationUnit)
        {
            if (compilationUnit == null)
                throw new ArgumentNullException(nameof(compilationUnit));

            _errorSink = errorSink;
            _compilationUnit = compilationUnit;

            _rootScope = compilationUnit.Scope;
            _scopes.Push(_rootScope);

             var children = new List<SyntaxNode>();

            foreach (var item in compilationUnit.Children)
                children.Add(item.Accept(this));

            compilationUnit = new CompilationUnit(compilationUnit, children, _rootScope);
        }

        protected override SyntaxNode VisitArithmetic(BinaryExpression expression)
        {
            var right = expression.Right.Accept(this) as Expression;
            var left = expression.Left.Accept(this) as Expression;

            return new BinaryExpression(expression.FilePart, left, right, expression.Operator);
        }
        protected override SyntaxNode VisitArrayAccess(ArrayAccessExpression expression)
        {
            var currentScope = _scopes.Peek();

            var arguments = new List<Expression>();

            var identifier = expression.Reference.Accept(this) as Expression;

            foreach (var item in expression.Arguments)
                arguments.Add(item.Accept(this) as Expression);

            return new ArrayAccessExpression(expression, identifier, arguments, identifier.Binding, currentScope);
        }
        protected override SyntaxNode VisitAssignment(BinaryExpression expression)
        {
            var right = expression.Right.Accept(this) as Expression;
            var left = expression.Left.Accept(this) as Expression;

            return new BinaryExpression(expression.FilePart, left, right, expression.Operator);
        }
        protected override SyntaxNode VisitBitwise(BinaryExpression expression)
        {
            var right = expression.Right.Accept(this) as Expression;
            var left = expression.Left.Accept(this) as Expression;

            return new BinaryExpression(expression.FilePart, left, right, expression.Operator);
        }
        protected override SyntaxNode VisitConstant(ConstantExpression expression)
        {
            return expression;
        }
        protected override SyntaxNode VisitIdentifier(IdentifierExpression expression)
        {
            return VisitIdentifier(expression, null);
        }
        protected override SyntaxNode VisitLambda(LambdaExpression expression)
        {
            throw new NotImplementedException();
        }
        protected override SyntaxNode VisitLogical(BinaryExpression expression)
        {
            var right = expression.Right.Accept(this) as Expression;
            var left = expression.Left.Accept(this) as Expression;

            return new BinaryExpression(expression.FilePart, left, right, expression.Operator);
        }
        protected override SyntaxNode VisitMethodCall(MethodCallExpression expression)
        {
            var currentScope = _scopes.Peek();

            var args = new List<Expression>();

            foreach (var item in expression.Arguments)
                args.Add(item.Accept(this) as Expression);

            var reference = expression.Reference.Accept(this) as Expression;

            return new MethodCallExpression(expression, reference, args, reference.Binding, currentScope);
        }
        protected override SyntaxNode VisitNew(NewExpression expression)
        {
            //switch(expression.Reference)
            //{
            //    case IdentifierExpression identifier:
            //
            //        break;
            //    default:
            //        AddError("Unsupported expression type for new statement", expression.FilePart);
            //        break;
            //}
            //throw new NotImplementedException();

            return expression;
        }
        protected override SyntaxNode VisitReference(ReferenceExpression expression)
        {
            // NOTE(Dan): Here we only need to check the first reference,
            //            We can do the subsequent parts when we actually resolve and check the types
            var first = expression.References.First();

            var identifier = first.Accept(this) as Expression;

            return new ReferenceExpression(expression.FilePart, new[] { identifier }.Union(expression.References.Skip(1)));

            //var references = VisitReference(expression.References.Skip(1), identifier);
            
            //return new ReferenceExpression(expression.FilePart, new [] { identifier }.Union(references));
        }
        protected override SyntaxNode VisitUnary(UnaryExpression expression)
        {
            var currentScope = _scopes.Peek();

            var argument = expression.Argument.Accept(this) as Expression;

            return new UnaryExpression(expression, argument, argument.Binding, currentScope);
        }

        protected override SyntaxNode VisitBlock(BlockStatement statement)
        {
            var currentScope = _scopes.Peek();

            var contents = new List<SyntaxNode>();

            foreach (var item in statement.Contents)
                contents.Add(item.Accept(this));

            return new BlockStatement(statement, contents, currentScope);
        }
        protected override SyntaxNode VisitBreak(BreakStatement statement)
        {
            return statement;
        }
        protected override SyntaxNode VisitCase(CaseStatement statement)
        {
            var currentScope = _scopes.Peek();
            var cases = new List<Expression>();
            var body = new List<SyntaxNode>();

            foreach (var item in statement.Cases)
                cases.Add(item.Accept(this) as Expression);

            foreach (var item in statement.Body)
                body.Add(item.Accept(this));

            return new CaseStatement(statement, cases, body, currentScope);
        }
        protected override SyntaxNode VisitContinue(ContinueStatement statement)
        {
            return statement;
        }
        protected override SyntaxNode VisitIf(IfStatement statement)
        {
            var currentScope = _scopes.Peek();
            var predicate = statement.Predicate.Accept(this) as Expression;
            var body = statement.Body.Accept(this) as BlockStatement;
            var @else = statement.ElseStatement.Accept(this) as ElseStatement;

            return new IfStatement(statement, predicate, body, @else, currentScope);
        }
        protected override SyntaxNode VisitElse(ElseStatement statement)
        {
            var currentScope = _scopes.Peek();
            var body = statement.Body.Accept(this) as BlockStatement;

            return new ElseStatement(statement, body, currentScope);
        }
        protected override SyntaxNode VisitEmpty(EmptyStatement statement)
        {
            return statement;
        }
        protected override SyntaxNode VisitFor(ForStatement statement)
        {
            var currentScope = _scopes.Peek();
            var initialiser = statement.Initialization?.Accept(this) as Expression;
            var condition = statement.Condition?.Accept(this) as Expression;
            var increment = statement.Increment?.Accept(this) as Expression;
            var body = statement.Body.Accept(this) as BlockStatement;

            return new ForStatement(statement, initialiser, condition, increment, body, currentScope);
        }
        protected override SyntaxNode VisitImport(ImportStatement statement)
        {
            var currentScope = _scopes.Peek();

            if (currentScope.TryGetValue(statement.Name, out ModuleDeclaration module))
            {
                // TODO(Dan): Import the symbols into the document scope so they can be used
                foreach (var item in module.Classes)
                    currentScope.AddClass(item.Name, item);

                foreach (var item in module.Methods)
                    currentScope.AddMethod(item.Name, item);
            }
            else
            {
                AddError($"Module '{statement.Name}' could not be found, are you missing an assembly reference?", statement.FilePart);
            }

            return statement;
        }
        protected override SyntaxNode VisitReturn(ReturnStatement statement)
        {
            var value = statement.Value?.Accept(this) as Expression;

            return new ReturnStatement(statement.FilePart, value);
        }
        protected override SyntaxNode VisitSwitch(SwitchStatement statement)
        {
            var currentScope = _scopes.Peek();
            var expression = statement.Condition.Accept(this) as Expression;

            var cases = new List<CaseStatement>();

            foreach (var item in statement.Cases)
                cases.Add(item.Accept(this) as CaseStatement);

            return new SwitchStatement(statement, expression, cases, currentScope);
        }
        protected override SyntaxNode VisitWhile(WhileStatement statement)
        {
            var currentScope = _scopes.Peek();

            var predicate = statement.Predicate.Accept(this) as Expression;
            var body = statement.Body.Accept(this) as BlockStatement;

            return new WhileStatement(statement, statement.IsDoWhile, predicate, body, currentScope);
        }

        protected override SyntaxNode VisitModuleDeclaration(ModuleDeclaration moduleDeclaration)
        {
            _currentModule = moduleDeclaration;

            var currentScope = _scopes.Peek();

            var scope = moduleDeclaration.Scope;

            _scopes.Push(scope);

            var classes = new List<ClassDeclaration>();
            var methods = new List<MethodDeclaration>();
            
            foreach (var item in moduleDeclaration.Classes)
                classes.Add(item.Accept(this) as ClassDeclaration);

            foreach(var item in moduleDeclaration.Methods)
                methods.Add(item.Accept(this) as MethodDeclaration);

            _scopes.Pop();

            return new ModuleDeclaration(moduleDeclaration, classes, methods, scope);
        }
        protected override SyntaxNode VisitClass(ClassDeclaration classDeclaration)
        {
            var scope = classDeclaration.Scope;

            _scopes.Push(scope);

            var fields = new List<FieldDeclaration>();
            var properties = new List<PropertyDeclaration>();
            var methods = new List<MethodDeclaration>();
            var constructors = new List<ConstructorDeclaration>();

            foreach (var item in classDeclaration.Fields)
                fields.Add(item.Accept(this) as FieldDeclaration);

            foreach (var item in classDeclaration.Properties)
                properties.Add(item.Accept(this) as PropertyDeclaration);

            foreach (var item in classDeclaration.Methods)
                methods.Add(item.Accept(this) as MethodDeclaration);

            foreach (var item in classDeclaration.Constructors)
                constructors.Add(item.Accept(this) as ConstructorDeclaration);

            _scopes.Pop();

            return new ClassDeclaration(classDeclaration,
                fields, properties, methods, constructors, scope);
        }
        protected override SyntaxNode VisitConstructor(ConstructorDeclaration constructorDeclaration)
        {
            var currentScope = _scopes.Peek();
            var scope = constructorDeclaration.Scope;

            _scopes.Push(scope);

            var body = constructorDeclaration.Body.Accept(this) as BlockStatement;

            _scopes.Pop();

            return new ConstructorDeclaration(constructorDeclaration, constructorDeclaration.Parameters, body, scope);
        }
        protected override SyntaxNode VisitField(FieldDeclaration fieldDeclaration)
        {
            var currentScope = _scopes.Peek();

            var defaultValue = fieldDeclaration.DefaultValue?.Accept(this) as Expression;

            return new FieldDeclaration(fieldDeclaration, defaultValue, currentScope);
        }
        protected override SyntaxNode VisitMethod(MethodDeclaration methodDeclaration)
        {
            var currentScope = _scopes.Peek();

            var scope = methodDeclaration.Scope;

            _scopes.Push(scope);

            var parameters = new List<ParameterDeclaration>();

            foreach (var item in methodDeclaration.Parameters)
                parameters.Add(item.Accept(this) as ParameterDeclaration);

            var body = methodDeclaration.Body.Accept(this) as BlockStatement;

            var declaration = new MethodDeclaration(methodDeclaration, parameters, body, scope);

            _scopes.Pop();

            return declaration;
        }
        protected override SyntaxNode VisitParameter(ParameterDeclaration parameterDeclaration)
        {
            var currentScope = _scopes.Peek();

            return parameterDeclaration;
        }
        protected override SyntaxNode VisitProperty(PropertyDeclaration propertyDeclaration)
        {
            var currentScope = _scopes.Peek();

            var getMethod = propertyDeclaration.GetMethod.Accept(this) as MethodDeclaration;
            var setMethod = propertyDeclaration.SetMethod?.Accept(this) as MethodDeclaration;

            return new PropertyDeclaration(propertyDeclaration, getMethod, setMethod, currentScope);
        }
        protected override SyntaxNode VisitType(TypeDeclaration typeDeclaration)
        {
            throw new NotImplementedException();
        }
        protected override SyntaxNode VisitVariable(VariableDeclaration variableDeclaration)
        {
            var currentScope = _scopes.Peek();

            var value = variableDeclaration.Value?.Accept(this) as Expression;
            var declaration = new VariableDeclaration(variableDeclaration, value, currentScope);

            if (currentScope.ContainsVariable(variableDeclaration.Name))
            {
                AddError($"Cannot re-declare variable '{variableDeclaration.Name}'", variableDeclaration.FilePart);
            }
            else
            {
                currentScope.AddVariable(declaration.Name, declaration);
            }

            return declaration;
        }

        protected override SyntaxNode VisitDocument(SourceDocument sourceDocument)
        {
            _currentDocument = sourceDocument;

            var currentScope = _scopes.Peek();

            var scope = sourceDocument.Scope;

            _scopes.Push(scope);

            foreach(var item in sourceDocument.Imports)
            {
                item.Accept(this);
            }

            var modules = new List<ModuleDeclaration>();

            foreach (var item in sourceDocument.Modules)
                modules.Add(item.Accept(this) as ModuleDeclaration);

            _scopes.Pop();

            return new SourceDocument(sourceDocument, modules, scope);
        }

        private IEnumerable<Expression> VisitReference(IEnumerable<Expression> references, Expression hint)
        {
            // TODO(Dan): Get hint declaration (binding)
            //            Take the hints type declaration
            //            Lookup the type definition
            //            if the type contains the first reference recurse
            //            if not, add an appropriate error!

            TypeDeclaration type = null;

            switch(hint)
            {
                case IdentifierExpression identifier:
                    {
                        switch(identifier.Binding)
                        {
                            case FieldDeclaration declaration:
                                type = declaration.Type;
                                break;
                            case PropertyDeclaration declaration:
                                type = declaration.Type;
                                break;
                            case MethodDeclaration declaration:
                                type = declaration.ReturnType;
                                break;
                        }
                    }
                    break;
                case MethodCallExpression methodCall:
                    {
                        switch (methodCall.Binding)
                        {
                            case FieldDeclaration declaration:
                                type = declaration.Type;
                                break;
                            case PropertyDeclaration declaration:
                                type = declaration.Type;
                                break;
                            case MethodDeclaration declaration:
                                type = declaration.ReturnType;
                                break;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (type == null)
                throw new InvalidOperationException();

            // TODO(Dan): If it's a built in type we need to handle it!
            if (type.IsBuiltInType())
            {

            }

            // NOTE(Dan): Start by checking our own document
            if (_currentDocument.Scope.TryGetValue(type.Name, out ClassDeclaration @class))
            {
                var newHint = VisitIdentifier(references.First() as IdentifierExpression, @class) as Expression;

                if (newHint.Binding == null)
                    return new[] { newHint };

                var additionalReferences = references.Skip(1);

                if (additionalReferences.Any())
                    return VisitReference(additionalReferences, newHint);

                return new[] { newHint };
            }

            return null;
            // We may not find it, in which case we need to check via the imports / imported modules!
        }
        private SyntaxNode VisitIdentifier(IdentifierExpression expression, Declaration hint)
        {
            var currentScope = hint == null
                ? _scopes.Peek()
                : hint.Scope;

            if (!currentScope.ContainsVariable(expression.Identifier) &&
                !currentScope.ContainsField(expression.Identifier) &&
                !currentScope.ContainsProperty(expression.Identifier) &&
                !currentScope.ContainsMethod(expression.Identifier))
            {
                if (hint == null)
                    AddError($"The name '{expression.Identifier}' does not exist in the current context", expression.FilePart);
                else
                    AddError($"The type '{hint.Name}' does not contain a declaration for '{expression.Identifier}'", expression.FilePart);
            }

            Declaration binding = null;

            // TODO(Dan): Bind the declaration to the usage
            if (currentScope.TryGetValue(expression.Identifier, out Declaration declaration))
            {
                binding = declaration;
                // This is either a variable declaration or a parameter declaration
            }
            if (currentScope.TryGetValue(expression.Identifier, out MethodDeclaration method))
            {
                binding = method;
                // This is a matching method declaration
            }
            if (currentScope.TryGetValue(expression.Identifier, out FieldDeclaration field))
            {
                binding = field;
                // This is either a variable declaration or a parameter declaration
            }
            if (currentScope.TryGetValue(expression.Identifier, out PropertyDeclaration property))
            {
                binding = property;
                // This is either a variable declaration or a parameter declaration
            }

            return new IdentifierExpression(expression, binding, currentScope);
        }

        private void AddError(string message, ISourceFilePart part)
        {
            var line = _currentDocument.SourceCode.Lines.Skip(part.Start.Line - 1).Take(1).ToArray();
            _errorSink.AddError(message, new SourceFilePart(_currentDocument.FilePart.FileName, line, part.Start, part.End), Severity.Error);
        }
        private void AddWarning(string message, ISourceFilePart part)
        {
            _errorSink.AddError(message, part, Severity.Warning);
        }
        private void AddInfo(string message, ISourceFilePart part)
        {
            _errorSink.AddError(message, part, Severity.Message);
        }

        public DeclarationPass()
        {
            _scopes = new Stack<Scope>();
        }
    }
}
