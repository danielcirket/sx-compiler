using FluentAssertions;
using Xunit;
using Sx.Lexer;

namespace Sx.Compiler.Lexer.Tests
{
    public class CharExtensionsTests
    {
        public class IsEOF
        {
            [Fact]
            public void IfCharIsEOFThenShouldReturnTrue()
            {
                var input = '\0';

                var result = input.IsEOF();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsNotEOFThenShouldReturnFalse()
            {
                var input = 'a';

                var result = input.IsEOF();

                result.Should().Be(false);
            }
        }

        public class IsDigit
        {
            [Fact]
            public void IfCharIsDigitThenShouldReturnTrue()
            {
                var input = '1';

                var result = input.IsDigit();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsNotDigitThenShouldReturnFalse()
            {
                var input = 'a';

                var result = input.IsEOF();

                result.Should().Be(false);
            }
        }

        public class IsLetter
        {
            [Fact]
            public void IfCharIsLetterThenShouldReturnTrue()
            {
                var input = 'a';

                var result = input.IsLetter();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsNotLetterThenShouldReturnFalse()
            {
                var input = '1';

                var result = input.IsLetter();

                result.Should().Be(false);
            }
        }
        public class IsLetterOrDigit
        {
            [Fact]
            public void IfCharIsDigitThenShouldReturnTrue()
            {
                var input = '1';

                var result = input.IsLetterOrDigit();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsLetterThenShouldReturnTrue()
            {
                var input = 'a';

                var result = input.IsLetterOrDigit();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsNotDigitOrLetterThenShouldReturnFalse()
            {
                var input = '>';

                var result = input.IsLetterOrDigit();

                result.Should().Be(false);
            }
        }
        public class IsNewLine
        {
            [Fact]
            public void IfCharIsNewLineThenShouldReturnTrue()
            {
                var input = '\n';

                var result = input.IsNewLine();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsNotNewLineThenShouldReturnFalse()
            {
                var input = 'a';

                var result = input.IsNewLine();

                result.Should().Be(false);
            }
        }
        public class IsWhiteSpace
        {
            [Fact]
            public void IfCharIsWhiteSpaceThenShouldReturnTrue()
            {
                var input = '\t';

                var result = input.IsWhiteSpace();

                result.Should().Be(true);
            }
            [Fact]
            public void IfCharIsNotWhiteSpaceThenShouldReturnFalse()
            {
                var input = 'a';

                var result = input.IsWhiteSpace();

                result.Should().Be(false);
            }
        }
        public class IsPunctuation
        {
            [Fact]
            public void IfCharIsPunctuationThenShouldReturnTrue()
            {
                var input = new[] { '<', ',', '>', '{', '}', '(', ')', '[', ']', '!', '%', '^', '&', '*', '+', '-', '=', '/', '.', ',', '?', ';', ':', '|' };

                foreach(var character in input)
                {
                    var result = character.IsPunctuation();
                    result.Should().Be(true);
                }
            }
            [Fact]
            public void IfCharIsNotPunctuationThenShouldReturnFalse()
            {
                var input = 'a';

                var result = input.IsPunctuation();

                result.Should().Be(false);
            }
        }
        public class IsIdentifier
        {
            [Fact]
            public void IfCharIsIdentifierThenShouldReturnTrue()
            {
                var input = new[] { '_', 'a', 'b', 'c', '1', '2', '3' };

                foreach (var character in input)
                {
                    var result = character.IsIdentifier();
                    result.Should().Be(true);
                }
                
            }
            [Fact]
            public void IfCharIsNotIdentifierThenShouldReturnFalse()
            {
                var input = '-';

                var result = input.IsIdentifier();

                result.Should().Be(false);
            }
        }
    }
}
