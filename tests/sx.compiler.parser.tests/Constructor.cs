using System;
using FluentAssertions;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Lexer.Abstractions;
using Sx.Lexer;
using Xunit;

namespace Sx.Compiler.Parser.Tests
{
    public partial class ParserTests
    {
        public class Constructor
        {
            [Fact]
            public void WhenDefaultConstructorCalledThenShouldNotThrowArgumentNullException()
            {
                Action act = () =>
                {
                    var parser = new SyntaxParser();
                };

                act.ShouldNotThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenOptionsAreNullThenShouldThrowArgumentNullException()
            {
                Action act = () =>
                {
                    var parser = new SyntaxParser(options: null, tokenizer: new Tokenizer(TokenizerGrammar.Default, new ErrorSink()), errorSink: new ErrorSink());
                };

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenTokenizerIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () =>
                {
                    var parser = new SyntaxParser(options: (o) => { }, tokenizer: null, errorSink: new ErrorSink());
                };

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenErrorSinkIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () =>
                {
                    var parser = new SyntaxParser(options: (o) => { }, tokenizer: new Tokenizer(TokenizerGrammar.Default, new ErrorSink()), errorSink: null);
                };

                act.ShouldThrow<ArgumentNullException>();
            }
        }
    }
}
