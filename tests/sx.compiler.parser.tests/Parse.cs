using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Syntax;
using Sx.Compiler.Parser.Syntax.Declarations;
using Sx.Compiler.Parser.Syntax.Expressions;
using Sx.Lexer;
using Sxc.Compiler.Parser.Abstractions;
using Xunit;

namespace Sx.Compiler.Parser.Tests
{
    public partial class ParserTests
    {
        public class Parse
        { 
            private static SyntaxParser CreateDefaultParser() => new SyntaxParser();
            private static ISourceFile CreateSourceFile(string name, string contents)
            {
                return new SourceFile(name + ".sx", contents);
            }

            [Fact]
            public void WhenSuppliedSourceFileIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () =>
                {
                    var parser = CreateDefaultParser();

                    parser.Parse(sourceFile: null);
                };

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenSuppliedSourceFilesIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () =>
                {
                    var parser = CreateDefaultParser();

                    parser.Parse(sourceFiles: null);
                };

                act.ShouldThrow<ArgumentNullException>();
            }

            public class ImportStatement
            {
                [Fact]
                public void WhenSuppliedWithNoImportStatementThenShouldNotThrow()
                {
                    Action act = () => 
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenSuppliedWithNoImportStatementThenShouldNotThrow), "module Test {}");

                        var result = parser.Parse(file);
                    };

                    act.ShouldNotThrow<SyntaxException>();
                }
                [Fact]
                public void WhenSuppliedWithNoImportStatementThenShouldReturnNoImportNodes()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithNoImportStatementThenShouldReturnNoImportNodes), "module Test {}");

                    var result = parser.Parse(file);

                    ((SourceDocument)(result.Children.First())).Imports.Any().Should().Be(false);
                }
                [Fact]
                public void WhenSuppliedWithImportStatementThenShouldReturnImportNode()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithImportStatementThenShouldReturnImportNode), "import Sx;");

                    var result = parser.Parse(file);

                    ((SourceDocument)(result.Children.First())).Imports.Count().Should().Be(1);
                }
                [Theory]
                [ClassData(typeof(MalformedImportNameTestCharacters))]
                public void WhenSuppliedMalformedNameThenShouldGiveSyntaxError(string invalidCharacter)
                {
                    var parser = CreateDefaultParser();
                    var moduleName = $"Sx.Something{invalidCharacter}AnotherThing";
                    var file = CreateSourceFile(nameof(WhenSuppliedMalformedNameThenShouldGiveSyntaxError), $"import {moduleName};");

                    var result = parser.Parse(file);

                    result.Should().BeNull();

                    parser.ErrorSink.HasErrors.Should().Be(true);
                    parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                    parser.ErrorSink.First().Message.Should().Contain($"Unexpected '{invalidCharacter}'. Expected '.' or ';'");
                }
                [Fact]
                public void WhenSuppliedWithManyImportStatementThenShouldReturnImportNodes()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithManyImportStatementThenShouldReturnImportNodes), "import Sx; import AnotherModule;");

                    var result = parser.Parse(file);

                    ((SourceDocument)(result.Children.First())).Imports.Count().Should().Be(2);
                }
                [Fact]
                public void WhenImportStatementNotAtStartOfFileThenShouldHaveUnexpectedTokenError()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenImportStatementNotAtStartOfFileThenShouldHaveUnexpectedTokenError), "module Sx {} import Sx;");

                    var result = parser.Parse(file);

                    parser.ErrorSink.HasErrors.Should().Be(true);
                    parser.ErrorSink.Errors.First().Message.Should().Be("Top-level statements are not permitted. Statements must be part of a module except for import statements which are at the top of the file in 'WhenImportStatementNotAtStartOfFileThenShouldHaveUnexpectedTokenError.sx'");
                    parser.ErrorSink.Errors.First().Severity.Should().Be(Severity.Error);
                }

                private class MalformedImportNameTestCharacters : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[] { "<" };
                        yield return new object[] { ">" };
                        yield return new object[] { "}" };
                        yield return new object[] { "(" };
                        yield return new object[] { ")" };
                        yield return new object[] { "[" };
                        yield return new object[] { "]" };
                        yield return new object[] { "!" };
                        yield return new object[] { "%" };
                        yield return new object[] { "^" };
                        yield return new object[] { "&" };
                        yield return new object[] { "*" };
                        yield return new object[] { "+" };
                        yield return new object[] { "-" };
                        yield return new object[] { "=" };
                        yield return new object[] { "/" };
                        yield return new object[] { "," };
                        yield return new object[] { "?" };
                        yield return new object[] { "{" };
                        yield return new object[] { "}" };
                        yield return new object[] { ":" };
                        yield return new object[] { "|" };
                    }

                    IEnumerator IEnumerable.GetEnumerator()
                    {
                        return GetEnumerator();
                    }
                }
            }

            public class ModuleStatement
            {
                [Fact]
                public void WhenSuppliedWithNoModuleStatementThenShouldNotThrow()
                {
                    Action act = () =>
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenSuppliedWithNoModuleStatementThenShouldNotThrow), "import Sx;");

                        var result = parser.Parse(file);
                    };

                    act.ShouldNotThrow<SyntaxException>();
                }
                [Fact]
                public void WhenSuppliedWithSingleIdentifierNameThenParsedModuleNameShouldMatch()
                {
                    var parser = CreateDefaultParser();
                    var moduleName = "Sx";
                    var file = CreateSourceFile(nameof(WhenSuppliedWithSingleIdentifierNameThenParsedModuleNameShouldMatch), $"module {moduleName} {{}}");

                    var result = parser.Parse(file);

                    result.Should().NotBeNull();
                    result.Should().BeOfType<CompilationUnit>();

                    ((SourceDocument)(result.Children.First())).Modules.Should().HaveCount(1);
                    ((SourceDocument)(result.Children.First())).Modules.First().Name.Should().Be(moduleName);
                }
                [Fact]
                public void WhenSuppliedWithNestedNameThenParsedModuleNameShouldMatch()
                {
                    var parser = CreateDefaultParser();
                    var moduleName = "Sx.Something.AnotherThing";
                    var file = CreateSourceFile(nameof(WhenSuppliedWithNestedNameThenParsedModuleNameShouldMatch), $"module {moduleName} {{}}");

                    var result = parser.Parse(file);

                    result.Should().NotBeNull();
                    result.Should().BeOfType<CompilationUnit>();

                    ((SourceDocument)(result.Children.First())).Modules.Should().HaveCount(1);
                    ((SourceDocument)(result.Children.First())).Modules.First().Name.Should().Be(moduleName);
                }
                [Theory]
                [ClassData(typeof(MalformedModuleNameTestCharacters))]
                public void WhenSuppliedMalformedNameThenShouldGiveSyntaxError(string invalidCharacter)
                {
                    var parser = CreateDefaultParser();
                    var moduleName = $"Sx.Something{invalidCharacter}AnotherThing";
                    var file = CreateSourceFile(nameof(WhenSuppliedMalformedNameThenShouldGiveSyntaxError), $"module {moduleName} {{}}");

                    var result = parser.Parse(file);

                    result.Should().BeNull();

                    parser.ErrorSink.HasErrors.Should().Be(true);
                    parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                    parser.ErrorSink.First().Message.Should().Contain($"Unexpected '{invalidCharacter}'. Expected '{{'");
                }
                [Fact]
                public void WhenSuppliedWithEmptyModuleStatementThenShouldReturnSourceDocumentNodeContainingEmptyModule()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithEmptyModuleStatementThenShouldReturnSourceDocumentNodeContainingEmptyModule), "module Sx {}");

                    var result = parser.Parse(file);

                    result.Should().NotBeNull();
                    result.Should().BeOfType<CompilationUnit>();

                    ((SourceDocument)(result.Children.First())).Modules.Should().HaveCount(1);
                    ((SourceDocument)(result.Children.First())).Modules.First().Classes.Should().HaveCount(0);
                    ((SourceDocument)(result.Children.First())).Modules.First().Methods.Should().HaveCount(0);
                }
                [Fact]
                public void WhenSuppliedWithModuleStatementContainingClassThenShouldReturnSourceDocumentNodeContainingModuleWithSingleChildClass()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithModuleStatementContainingClassThenShouldReturnSourceDocumentNodeContainingModuleWithSingleChildClass), "module Sx { public class TestClass {}}");

                    var result = parser.Parse(file);

                    result.Should().NotBeNull();
                    result.Should().BeOfType<CompilationUnit>();

                    ((SourceDocument)(result.Children.First())).Modules.Should().HaveCount(1);
                    ((SourceDocument)(result.Children.First())).Modules.First().Classes.Should().HaveCount(1);
                }
                [Fact]
                public void WhenSuppliedWithModuleStatementContainingMethodThenShouldReturnSourceDocumentNodeContainingModuleWithSingleChildMethod()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithModuleStatementContainingMethodThenShouldReturnSourceDocumentNodeContainingModuleWithSingleChildMethod), "module Sx { public void Test() {}}");

                    var result = parser.Parse(file);

                    result.Should().NotBeNull();
                    result.Should().BeOfType<CompilationUnit>();

                    ((SourceDocument)(result.Children.First())).Modules.Should().HaveCount(1);
                    ((SourceDocument)(result.Children.First())).Modules.First().Methods.Should().HaveCount(1);
                }

                private class MalformedModuleNameTestCharacters : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[] { "<" };
                        yield return new object[] { ">" };
                        yield return new object[] { "}" };
                        yield return new object[] { "(" };
                        yield return new object[] { ")" };
                        yield return new object[] { "[" };
                        yield return new object[] { "]" };
                        yield return new object[] { "!" };
                        yield return new object[] { "%" };
                        yield return new object[] { "^" };
                        yield return new object[] { "&" };
                        yield return new object[] { "*" };
                        yield return new object[] { "+" };
                        yield return new object[] { "-" };
                        yield return new object[] { "=" };
                        yield return new object[] { "/" };
                        yield return new object[] { "," };
                        yield return new object[] { "?" };
                        yield return new object[] { "}" };
                        yield return new object[] { ":" };
                        yield return new object[] { "|" };
                    }

                    IEnumerator IEnumerable.GetEnumerator()
                    {
                        return GetEnumerator();
                    }
                }
            }

            public class ClassStatement
            {
                [Fact]
                public void WhenSuppliedWithNoClassStatementThenShouldNotThrow()
                {
                    Action act = () =>
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenSuppliedWithNoClassStatementThenShouldNotThrow), "module Sx {}");

                        var result = parser.Parse(file);
                    };

                    act.ShouldNotThrow<SyntaxException>();
                }
                [Theory]
                [ClassData(typeof(MalformedClassNameTestCharacters))]
                public void WhenSuppliedMalformedNameThenShouldGiveSyntaxError(string invalidCharacter)
                {
                    var parser = CreateDefaultParser();
                    var className = $"MyClass{invalidCharacter}Name";
                    var file = CreateSourceFile(nameof(WhenSuppliedMalformedNameThenShouldGiveSyntaxError), $"module Sx {{ public class {className} {{}}}}");

                    var result = parser.Parse(file);

                    parser.ErrorSink.HasErrors.Should().Be(true);
                    parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                    parser.ErrorSink.First().Message.Should().StartWith($"Unexpected ");
                }
                [Fact]
                public void WhenSuppliedWithValidEmptyClassStatementThenShouldReturnClassNode()
                {
                    var parser = CreateDefaultParser();
                    var file = CreateSourceFile(nameof(WhenSuppliedWithValidEmptyClassStatementThenShouldReturnClassNode), "module Sx { public class TestClass {}}");

                    var result = parser.Parse(file);

                    var moduleDeclaration  = ((SourceDocument)(result.Children.First())).Modules.First();
                    moduleDeclaration.Classes.Should().HaveCount(1);
                }

                public class Field
                {
                    [Fact]
                    public void WhenSuppliedWithNoFieldStatementsThenShouldNotThrow()
                    {
                        Action act = () =>
                        {
                            var parser = CreateDefaultParser();
                            var file = CreateSourceFile(nameof(WhenSuppliedWithNoClassStatementThenShouldNotThrow), "module Sx { public class TestClass {}}");

                            var result = parser.Parse(file);
                        };

                        act.ShouldNotThrow<SyntaxException>();
                    }
                    [Theory]
                    [ClassData(typeof(MalformedFieldNameTestCharacters))]
                    public void WhenDeclaredWithMalformedNameThenShouldGiveSyntaxError(string invalidCharacter)
                    {
                        var parser = CreateDefaultParser();
                        var fieldName = $"_field{invalidCharacter}Name";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMalformedNameThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ private int {fieldName}; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected ");
                    }
                    [Fact]
                    public void WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var fieldName = $"_fieldName";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError), $"module Sx {{ public class TestClass {{ T {fieldName}; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected 'T'. Expected Visibility modifier, 'public', 'protected', 'private'");
                    }
                    [Fact]
                    public void WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var fieldName = $"_fieldName";
                        var file = CreateSourceFile(nameof(WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ private {fieldName}; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected ';'. Expected Type keyword or identifier");
                    }
                    [Fact]
                    public void WhenValidNonAssignmentDeclarationThenShouldHaveNoErrors()
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenValidNonAssignmentDeclarationThenShouldHaveNoErrors), "module Sx { public class TestClass { private T _fieldName; } }");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var fieldDeclaration = classDeclaration.Fields.First();

                        fieldDeclaration.Name.Should().Be("_fieldName");
                        fieldDeclaration.DefaultValue.Should().BeNull();
                        fieldDeclaration.Visibility.Should().Be(DeclarationVisibility.Private);
                        fieldDeclaration.Type.Name.Should().Be("T");
                    }
                    [Fact]
                    public void WhenValidAssignmentFromMethodDeclarationThenShouldHaveNoErrors()
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenValidAssignmentFromMethodDeclarationThenShouldHaveNoErrors), "module Sx { public class TestClass { private T _fieldName = SomeMethod(); } }");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);
                    }
                    [Fact]
                    public void WhenValidAssignmentFromConstantDeclarationThenShouldHaveNoErrors()
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenValidAssignmentFromConstantDeclarationThenShouldHaveNoErrors), "module Sx { public class TestClass { private T _fieldName = 1; } }");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);
                    }
                    [Fact]
                    public void WhenValidAssignedWithMethodDeclarationThenShouldHaveDefaultValue()
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenValidAssignedWithMethodDeclarationThenShouldHaveDefaultValue), "module Sx { public class TestClass { private T _fieldName = SomeMethod(); } }");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var fieldDeclaration = classDeclaration.Fields.First();
                        
                        fieldDeclaration.Name.Should().Be("_fieldName");
                        fieldDeclaration.DefaultValue.Should().BeOfType<MethodCallExpression>();
                        fieldDeclaration.Visibility.Should().Be(DeclarationVisibility.Private);
                        fieldDeclaration.Type.Name.Should().Be("T");
                    }
                    [Fact]
                    public void WhenValidAssignedWithConstantDeclarationThenShouldHaveDefaultValue()
                    {
                        var parser = CreateDefaultParser();
                        var file = CreateSourceFile(nameof(WhenValidAssignedWithConstantDeclarationThenShouldHaveDefaultValue), "module Sx { public class TestClass { private T _fieldName = 1; } }");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var fieldDeclaration = classDeclaration.Fields.First();

                        fieldDeclaration.Name.Should().Be("_fieldName");
                        fieldDeclaration.DefaultValue.Should().BeOfType<ConstantExpression>();
                        fieldDeclaration.Visibility.Should().Be(DeclarationVisibility.Private);
                        fieldDeclaration.Type.Name.Should().Be("T");
                    }
                    [Fact]
                    public void WhenValidAssignmentUsingLambdaWithNoArgsThenShouldHaveNoErrors()
                    {
                        var parser = CreateDefaultParser();
                        var fieldName = $"_fieldName";
                        var file = CreateSourceFile(nameof(WhenValidAssignmentUsingLambdaWithNoArgsThenShouldHaveNoErrors), $"module Sx {{ public class TestClass {{ private T {fieldName} = () => {{ return 1; }}; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var fieldDeclaration = classDeclaration.Fields.First();

                        fieldDeclaration.DefaultValue.Should().BeOfType<LambdaExpression>();
                    }
                    [Fact]
                    public void WhenValidAssignmentUsingLambdaWithArgsThenShouldHaveNoErrors()
                    {
                        var parser = CreateDefaultParser();
                        var fieldName = $"_fieldName";
                        var file = CreateSourceFile(nameof(WhenValidAssignmentUsingLambdaWithArgsThenShouldHaveNoErrors), $"module Sx {{ public class TestClass {{ private T {fieldName} = () => {{ return 1; }}; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var fieldDeclaration = classDeclaration.Fields.First();

                        fieldDeclaration.DefaultValue.Should().BeOfType<LambdaExpression>();
                    }
                    [Fact]
                    public void WhenValidAssignmentUsingLambdaUsedAsMethodCallThenShouldHaveError()
                    {
                        var parser = CreateDefaultParser();
                        var fieldName = $"_fieldName";
                        var file = CreateSourceFile(nameof(WhenValidAssignmentUsingLambdaWithNoArgsThenShouldHaveNoErrors), $"module Sx {{ public class TestClass {{ private T {fieldName} = () => {{ return 1; }}(); }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.Errors.First().Message.Should().StartWith("Unexpected '('. Expected ';'");
                    }

                    private class MalformedFieldNameTestCharacters : IEnumerable<object[]>
                    {
                        public IEnumerator<object[]> GetEnumerator()
                        {
                            yield return new object[] { "<" };
                            yield return new object[] { ">" };
                            yield return new object[] { "}" };
                            yield return new object[] { "(" };
                            yield return new object[] { ")" };
                            yield return new object[] { "[" };
                            yield return new object[] { "]" };
                            yield return new object[] { "!" };
                            yield return new object[] { "%" };
                            yield return new object[] { "^" };
                            yield return new object[] { "&" };
                            yield return new object[] { "*" };
                            yield return new object[] { "+" };
                            yield return new object[] { "-" };
                            yield return new object[] { "/" };
                            yield return new object[] { "," };
                            yield return new object[] { "?" };
                            yield return new object[] { "{" };
                            yield return new object[] { "}" };
                            yield return new object[] { ":" };
                            yield return new object[] { "|" };
                        }

                        IEnumerator IEnumerable.GetEnumerator()
                        {
                            return GetEnumerator();
                        }
                    }
                }

                public class Property
                {
                    [Fact]
                    public void WhenSuppliedWithNoPropertyStatementsThenShouldNotThrow()
                    {
                        Action act = () =>
                        {
                            var parser = CreateDefaultParser();
                            var file = CreateSourceFile(nameof(WhenSuppliedWithNoClassStatementThenShouldNotThrow), "module Sx { public class TestClass {}}");

                            var result = parser.Parse(file);
                        };

                        act.ShouldNotThrow<SyntaxException>();
                    }
                    [Theory]
                    [ClassData(typeof(MalformedPropertyNameTestCharacters))]
                    public void WhenDeclaredWithMalformedNameThenShouldGiveSyntaxError(string invalidCharacter)
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"Property{invalidCharacter}Name";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMalformedNameThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ private int {propertyName} => x; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected ");
                    }
                    [Fact]
                    public void WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError), $"module Sx {{ public class TestClass {{ T {propertyName} => x; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected 'T'. Expected Visibility modifier, 'public', 'protected', 'private'");
                    }
                    [Fact]
                    public void WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public {propertyName} => x; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected '=>'. Expected Type keyword or identifier");
                    }
                    [Fact]
                    public void WhenValidExpressionBodiedDeclarationThenShouldGiveNoSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenValidExpressionBodiedDeclarationThenShouldGiveNoSyntaxError), $"module Sx {{ public class TestClass {{ public T {propertyName} => x; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);
                    }
                    [Fact]
                    public void WhenValidExpressionBodiedDeclarationThenShouldHaveNonNullGetMethod()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenValidExpressionBodiedDeclarationThenShouldHaveNonNullGetMethod), $"module Sx {{ public class TestClass {{ public T {propertyName} => x; }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var propertyDeclaration = classDeclaration.Properties.First();

                        propertyDeclaration.GetMethod.Should().NotBeNull();
                        propertyDeclaration.GetMethod.Name.Should().Be($"get_{propertyName}");
                    }
                    [Fact]
                    public void WhenValidGetExpressionBodiedDeclarationsThenShouldHaveNonNullGetMethod()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenValidGetExpressionBodiedDeclarationsThenShouldHaveNonNullGetMethod), $"module Sx {{ public class TestClass {{ public T {propertyName} {{ get => x; }} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var propertyDeclaration = classDeclaration.Properties.First();

                        propertyDeclaration.GetMethod.Should().NotBeNull();
                        propertyDeclaration.GetMethod.Name.Should().Be($"get_{propertyName}");
                    }
                    [Fact]
                    public void WhenValidSetExpressionBodiedDeclarationsThenShouldHaveNonNullGetMethod()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenValidGetExpressionBodiedDeclarationsThenShouldHaveNonNullGetMethod), $"module Sx {{ public class TestClass {{ public T {propertyName} {{ get => x; set => x = value; }} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var propertyDeclaration = classDeclaration.Properties.First();

                        propertyDeclaration.SetMethod.Should().NotBeNull();
                        propertyDeclaration.SetMethod.Name.Should().Be($"set_{propertyName}");
                    }
                    [Fact]
                    public void WhenMissingGetterThenShouldHaveSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenMissingGetterThenShouldHaveSyntaxError), $"module Sx {{ public class TestClass {{ public T {propertyName} {{ set => x = value; }} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.Errors.First().Message.Should().StartWith($"Property \"{propertyName}\" does not have a getter in ");
                    }
                    [Fact]
                    public void WhenValidGetterAndSetterThenShouldHaveNonNullGetAndSetMethods()
                    {
                        var parser = CreateDefaultParser();
                        var propertyName = $"PropertyName";
                        var file = CreateSourceFile(nameof(WhenValidGetExpressionBodiedDeclarationsThenShouldHaveNonNullGetMethod), $"module Sx {{ public class TestClass {{ public T {propertyName} {{ get => x; set => x = value; }} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var propertyDeclaration = classDeclaration.Properties.First();

                        propertyDeclaration.GetMethod.Should().NotBeNull();
                        propertyDeclaration.GetMethod.Name.Should().Be($"get_{propertyName}");

                        propertyDeclaration.SetMethod.Should().NotBeNull();
                        propertyDeclaration.SetMethod.Name.Should().Be($"set_{propertyName}");
                    }

                    private class MalformedPropertyNameTestCharacters : IEnumerable<object[]>
                    {
                        public IEnumerator<object[]> GetEnumerator()
                        {
                            yield return new object[] { "<" };
                            yield return new object[] { ">" };
                            yield return new object[] { "}" };
                            yield return new object[] { "(" };
                            yield return new object[] { ")" };
                            yield return new object[] { "[" };
                            yield return new object[] { "]" };
                            yield return new object[] { "!" };
                            yield return new object[] { "%" };
                            yield return new object[] { "^" };
                            yield return new object[] { "&" };
                            yield return new object[] { "*" };
                            yield return new object[] { "+" };
                            yield return new object[] { "-" };
                            yield return new object[] { "/" };
                            yield return new object[] { "," };
                            yield return new object[] { "?" };
                            yield return new object[] { "{" };
                            yield return new object[] { "}" };
                            yield return new object[] { ":" };
                            yield return new object[] { "|" };
                        }

                        IEnumerator IEnumerable.GetEnumerator()
                        {
                            return GetEnumerator();
                        }
                    }
                }

                public class Method
                {
                    [Fact]
                    public void WhenSuppliedWithNoMethodStatementsThenShouldNotThrow()
                    {
                        Action act = () =>
                        {
                            var parser = CreateDefaultParser();
                            var file = CreateSourceFile(nameof(WhenSuppliedWithNoClassStatementThenShouldNotThrow), "module Sx { public class TestClass {}}");

                            var result = parser.Parse(file);
                        };

                        act.ShouldNotThrow<SyntaxException>();
                    }
                    [Theory]
                    [ClassData(typeof(MalformedMethodNameTestCharacters))]
                    public void WhenDeclaredWithMalformedNameThenShouldGiveSyntaxError(string invalidCharacter)
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"Method{invalidCharacter}Name";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMalformedNameThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public int {methodName}() {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected ");
                    }
                    [Fact]
                    public void WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"MethodName";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError), $"module Sx {{ public class TestClass {{ T {methodName} {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected 'T'. Expected Visibility modifier, 'public', 'protected', 'private'");
                    }
                    [Fact]
                    public void WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"MethodName";
                        var file = CreateSourceFile(nameof(WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public {methodName} {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected '{{'. Expected Type keyword or identifier");
                    }
                    [Fact]
                    public void WhenMissingMethodBodyThenShouldGiveSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"MethodName";
                        var file = CreateSourceFile(nameof(WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public T {methodName} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected '}}'. Expected Field, Property or Method Declaration");
                    }
                    [Fact]
                    public void WhenEmptyBodyThenShouldParse()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"MethodName";
                        var file = CreateSourceFile(nameof(WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public T {methodName}() {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var methodDeclaration = classDeclaration.Methods.First();

                        methodDeclaration.Name.Should().Be(methodName);
                        methodDeclaration.ReturnType.Name.Should().Be("T");
                        methodDeclaration.Parameters.Should().HaveCount(0);
                        methodDeclaration.Body.Contents.Should().HaveCount(0);
                    }
                    [Fact]
                    public void WhenEmptyBodyWithMethodParameterThenShouldParse()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"MethodName";
                        var file = CreateSourceFile(nameof(WhenMissingTypeOrIdentifierThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public T {methodName}(T a) {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var methodDeclaration = classDeclaration.Methods.First();

                        methodDeclaration.Name.Should().Be(methodName);
                        methodDeclaration.ReturnType.Name.Should().Be("T");
                        methodDeclaration.Parameters.Should().HaveCount(1);
                        methodDeclaration.Parameters.First().Name.Should().Be("a");
                        methodDeclaration.Parameters.First().Type.Name.Should().Be("T");
                        methodDeclaration.Body.Contents.Should().HaveCount(0);
                    }

                    private class MalformedMethodNameTestCharacters : IEnumerable<object[]>
                    {
                        public IEnumerator<object[]> GetEnumerator()
                        {
                            yield return new object[] { "<" };
                            yield return new object[] { ">" };
                            yield return new object[] { "}" };
                            yield return new object[] { "(" };
                            yield return new object[] { ")" };
                            yield return new object[] { "[" };
                            yield return new object[] { "]" };
                            yield return new object[] { "!" };
                            yield return new object[] { "%" };
                            yield return new object[] { "^" };
                            yield return new object[] { "&" };
                            yield return new object[] { "*" };
                            yield return new object[] { "+" };
                            yield return new object[] { "-" };
                            yield return new object[] { "/" };
                            yield return new object[] { "," };
                            yield return new object[] { "?" };
                            yield return new object[] { "{" };
                            yield return new object[] { "}" };
                            yield return new object[] { ":" };
                            yield return new object[] { "|" };
                        }

                        IEnumerator IEnumerable.GetEnumerator()
                        {
                            return GetEnumerator();
                        }
                    }
                }

                public class Constructor
                {
                    // constructor declaration = <visibility>, "constructor", <argument list>, <scope>
                    [Fact]
                    public void WhenSuppliedWithNoConstructorStatementsThenShouldNotThrow()
                    {
                        Action act = () =>
                        {
                            var parser = CreateDefaultParser();
                            var file = CreateSourceFile(nameof(WhenSuppliedWithNoConstructorStatementsThenShouldNotThrow), "module Sx { public class TestClass {}}");

                            var result = parser.Parse(file);
                        };

                        act.ShouldNotThrow<SyntaxException>();
                    }
                    [Fact]
                    public void WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"constructor";
                        var file = CreateSourceFile(nameof(WhenDeclaredWithMissingVisibilityThenShouldGiveMissingVisibilityModifierSyntaxError), $"module Sx {{ public class TestClass {{ {methodName} {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected '{methodName}'. Expected Visibility modifier, 'public', 'protected', 'private'");
                    }
                    [Fact]
                    public void WhenMissingMethodBodyThenShouldGiveSyntaxError()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"constructor";
                        var file = CreateSourceFile(nameof(WhenMissingMethodBodyThenShouldGiveSyntaxError), $"module Sx {{ public class TestClass {{ public {methodName}() }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(true);
                        parser.ErrorSink.First().Severity.Should().Be(Severity.Error);

                        parser.ErrorSink.First().Message.Should().StartWith($"Unexpected '}}'. Expected '{{");
                    }
                    [Fact]
                    public void WhenEmptyBodyThenShouldParse()
                    {
                        var parser = CreateDefaultParser();
                        var methodName = $"constructor";
                        var file = CreateSourceFile(nameof(WhenEmptyBodyThenShouldParse), $"module Sx {{ public class TestClass {{ public {methodName}() {{}} }}}}");

                        var result = parser.Parse(file);

                        parser.ErrorSink.HasErrors.Should().Be(false);

                        var sourceDocument = (SourceDocument)(result.Children.First());
                        var moduleDeclaration = sourceDocument.Modules.First();
                        var classDeclaration = moduleDeclaration.Classes.First();
                        var constructorDeclaration = classDeclaration.Constructors.First();

                        constructorDeclaration.Name.Should().Be(methodName);
                        constructorDeclaration.Parameters.Should().HaveCount(0);
                        constructorDeclaration.Body.Contents.Should().HaveCount(0);
                    }
                }


                private class MalformedClassNameTestCharacters : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[] { "<" };
                        yield return new object[] { ">" };
                        yield return new object[] { "}" };
                        yield return new object[] { "(" };
                        yield return new object[] { ")" };
                        yield return new object[] { "[" };
                        yield return new object[] { "]" };
                        yield return new object[] { "!" };
                        yield return new object[] { "%" };
                        yield return new object[] { "^" };
                        yield return new object[] { "&" };
                        yield return new object[] { "*" };
                        yield return new object[] { "+" };
                        yield return new object[] { "-" };
                        yield return new object[] { "=" };
                        yield return new object[] { "/" };
                        yield return new object[] { "," };
                        yield return new object[] { "?" };
                        yield return new object[] { "{" };
                        yield return new object[] { "}" };
                        yield return new object[] { ":" };
                        yield return new object[] { "|" };
                    }

                    IEnumerator IEnumerable.GetEnumerator()
                    {
                        return GetEnumerator();
                    }
                }
            }

            public class While
            {
                
            }

            public class DoWhile
            {

            }

            public class For
            {

            }

            public class If
            {

            }

            public class IfElse
            {

            }

            public class Switch
            {

            }

            public class LambdaExpressions
            {

            }
        }
    }
}
