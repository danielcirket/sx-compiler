using FluentAssertions;
using Xunit;
using Sx.Lexer;
using System;

namespace Sx.Compiler.Lexer.Tests
{
    public partial class LexerTests
    {
        public class StringExtensionTests
        {
            public class IsKeyword
            {
                [Fact]
                public void IfSourceIsNullThenShouldThrowArgumentNullException()
                {
                    string input = null;

                    Action act = () => input.IsKeyword(new[] { "class" });

                    act.ShouldThrow<ArgumentNullException>();
                }
                [Fact]
                public void IfSourceExistsInKeywordsThenShouldReturnTrue()
                {
                    var input = "class";

                    var result = input.IsKeyword(new[] { input });

                    result.Should().Be(true);
                }
                [Fact]
                public void IfSourceDoesNotExistInKeywordsThenShouldReturnFalse()
                {
                    var input = "class";

                    var result = input.IsKeyword(new[] { "struct" });

                    result.Should().Be(false);
                }
            }
        }
    }
}
