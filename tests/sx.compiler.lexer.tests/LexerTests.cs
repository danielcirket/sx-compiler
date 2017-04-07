using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Lexer.Abstractions;
using Sx.Lexer.Abstractions;
using Xunit;

namespace Sx.Compiler.Lexer.Tests
{
    public class LexerTests
    {
        public class Constructor
        {
            [Fact]
            public void WhenGrammarIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () => new Sx.Lexer.Lexer(null);

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenErrorSinkIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () => new Sx.Lexer.Lexer(new TokenizerGrammar(), null);

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenGrammarAndErrorSinkAreNotNullThenShouldContructLexer()
            {
                var result = new Sx.Lexer.Lexer(new TokenizerGrammar());

                result.Should().NotBeNull();
            }
        }

        public class Tokenize
        {
            [Fact]
            public void WhenSourceFileIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () => new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize((ISourceFile)null);

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenSourceFileContentPassedAsStringIsNullThenShouldThrowArgumentNullException()
            {
                Action act = () => new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize((string)null);

                act.ShouldThrow<ArgumentNullException>();
            }
            [Fact]
            public void WhenSourceFileContainsEOFThenShouldReturnEOFToken()
            {
                var input = '\0'.ToString();

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(1);
                result[0].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsNewLineThenShouldReturnNewLineTokenAndEOFToken()
            {
                var input = '\n'.ToString();

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.NewLine);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsWhiteSpaceThenShouldReturnWhiteSpaceTokenAndEOFToken()
            {
                var input = '\t'.ToString();

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.Whitespace);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsDigitThenShouldReturnIntegerLiteralAndEOFToken()
            {
                var input = '1'.ToString();

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.IntegerLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsFloatThenShouldReturnRealLiteralAndEOFToken()
            {
                var input = "1f";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.RealLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsDecimalThenShouldReturnRealLiteralAndEOFToken()
            {
                var input = "1.0m";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.RealLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsDoubleThenShouldReturnRealLiteralAndEOFToken()
            {
                var input = "1.0";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.RealLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsFloatExponentThenShouldReturnRealLiteralAndEOFToken()
            {
                var input = "1e10f";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.RealLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsSingleLineCommentThenShouldReturnSingleLineCommentAndEOFToken()
            {
                var input = "# This is a single line comment";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.LineComment);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsMultiLineCommentThenShouldReturnBlockLineCommentAndEOFToken()
            {
                var input = @"/* This is a single line comment
                               * This is a second Line
                               */";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.BlockComment);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsIdentifierThenShouldReturnIdentifierAndEOFToken()
            {
                var input = @"identifier";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.Identifier);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsKeywordThenShouldReturnKeywordAndEOFToken()
            {
                var input = @"class";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar
                {
                    Keywords = new List<TokenMatch>
                    {
                        new TokenMatch(TokenType.Keyword, input)
                    }
                }).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.Keyword);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsCharLiteralThenShouldReturnCharLiteralAndEOFToken()
            {
                var input = @"'a'";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.CharLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsStringLiteralThenShouldReturnStringLiteralAndEOFToken()
            {
                var input = "\"string literal\"";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.StringLiteral);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsPunctuationThenShouldReturnMatchingPunctuationAndEOFToken()
            {
                var input = new[] { '<', '>', '{', '}', '(', ')', '[', ']', '!', '%', '^', '&', '*', '+', '-', '=', '/', '.', ',', '?', ';', ':', '|' };

                foreach (var character in input)
                {
                    var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(character.ToString()).ToList();
                    result.Count.Should().Be(2);

                    var matchingTokenType = GetPunctuationTokenType(character);
                    
                    result[0].TokenType.Should().Be(matchingTokenType);
                    result[1].TokenType.Should().Be(TokenType.EOF);
                }
            }

            [Fact]
            public void WhenSourceFileContainsInvalidIntTokenThenShouldReturnErrorAndEOFToken()
            {
                var input = "1z";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.Error);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsInvalidFloatTokenThenShouldReturnErrorAndEOFToken()
            {
                var input = "1ff";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.Error);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsUnRecognisedPunctuationTHenShouldReturnErrorAndEOFToken()
            {
                var input = "~";

                var result = new Sx.Lexer.Lexer(new TokenizerGrammar()).Tokenize(input).ToList();

                result.Count.Should().Be(2);
                result[0].TokenType.Should().Be(TokenType.Error);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }
            [Fact]
            public void WhenSourceFileContainsErrorsThenShouldReturnErrorsInErrorSinkAndEOFToken()
            {
                var input = "~";

                var lexer = new Sx.Lexer.Lexer(new TokenizerGrammar());

                var result = lexer.Tokenize(input).ToList();

                lexer.ErrorSink.HasErrors.Should().Be(true);
                lexer.ErrorSink.Errors.Count().Should().Be(1);
                result[0].TokenType.Should().Be(TokenType.Error);
                result[1].TokenType.Should().Be(TokenType.EOF);
            }

            private TokenType GetPunctuationTokenType(char character)
            {
                switch (character)
                {
                    //'<', '>', '{', '}', '(', ')', '[', ']', '!', '%', '^', '&', '*', '+', '-', '=', '/', '.', ',', '?', ';', ':', '|'
                    case '<':
                        return TokenType.LessThan;
                    case '>':
                        return TokenType.GreaterThan;
                    case '{':
                        return TokenType.LeftBracket;
                    case '}':
                        return TokenType.RightBracket;
                    case '(':
                        return TokenType.LeftParenthesis;
                    case ')':
                        return TokenType.RightParenthesis;
                    case '[':
                        return TokenType.LeftBrace;
                    case ']':
                        return TokenType.RightBrace;
                    case '!':
                        return TokenType.Not;
                    case '%':
                        return TokenType.Mod;
                    case '^':
                        return TokenType.BitwiseXor;
                    case '&':
                        return TokenType.BitwiseAnd;
                    case '*':
                        return TokenType.Mul;
                    case '+':
                        return TokenType.Plus;
                    case '-':
                        return TokenType.Minus;
                    case '=':
                        return TokenType.Assignment;
                    case '/':
                        return TokenType.Div;
                    case '.':
                        return TokenType.Dot;
                    case ',':
                        return TokenType.Comma;
                    case '?':
                        return TokenType.Question;
                    case ';':
                        return TokenType.Semicolon;
                    case ':':
                        return TokenType.Colon;
                    case '|':
                        return TokenType.BitwiseOr;
                    default:
                        throw new Exception("No match found");
                }
            }
        }
    }
}
