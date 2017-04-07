using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sx.Compiler.Lexer.Abstractions;
using Sx.Lexer;
using Sx.Lexer.Abstractions;

namespace Sx.Samples.Lexer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var content = File.ReadAllLines("./Input.sx");

            var lexer = new Sx.Lexer.Lexer(new TokenizerGrammar
            {
                Keywords = new List<TokenMatch>
                {
                    new TokenMatch(TokenType.Keyword, "import"),
                    new TokenMatch(TokenType.Keyword, "module"),
                    new TokenMatch(TokenType.Keyword, "public"),
                    new TokenMatch(TokenType.Keyword, "private"),
                    new TokenMatch(TokenType.Keyword, "internal"),
                    new TokenMatch(TokenType.Keyword, "protected"),
                    new TokenMatch(TokenType.Keyword, "class"),
                    new TokenMatch(TokenType.Keyword, "interface"),
                    new TokenMatch(TokenType.Keyword, "if"),
                    new TokenMatch(TokenType.Keyword, "else"),
                    new TokenMatch(TokenType.Keyword, "switch"),
                    new TokenMatch(TokenType.Keyword, "case"),
                    new TokenMatch(TokenType.Keyword, "default"),
                    new TokenMatch(TokenType.Keyword, "break"),
                    new TokenMatch(TokenType.Keyword, "return"),
                    new TokenMatch(TokenType.Keyword, "while"),
                    new TokenMatch(TokenType.Keyword, "for"),
                    new TokenMatch(TokenType.Keyword, "let"),
                    new TokenMatch(TokenType.Keyword, "true"),
                    new TokenMatch(TokenType.Keyword, "false"),
                    new TokenMatch(TokenType.Keyword, "int"),
                    new TokenMatch(TokenType.Keyword, "string"),
                    new TokenMatch(TokenType.Keyword, "void"),
                    new TokenMatch(TokenType.Keyword, "float"),
                    new TokenMatch(TokenType.Keyword, "double"),
                    new TokenMatch(TokenType.Keyword, "decimal"),
                    new TokenMatch(TokenType.Keyword, "char"),
                    new TokenMatch(TokenType.Keyword, "try"),
                    new TokenMatch(TokenType.Keyword, "catch"),

                },
                SpecialCharacters = new List<TokenMatch>
                {
                    new TokenMatch(TokenType.LineComment, "//"),
                    new TokenMatch(TokenType.LineComment, "#"),
                    new TokenMatch(TokenType.BlockComment, "/*"),
                    new TokenMatch(TokenType.BlockComment, "*/"),
                    new TokenMatch(TokenType.LeftBracket, "{"),
                    new TokenMatch(TokenType.RightBracket, "}"),
                    new TokenMatch(TokenType.LeftBrace, "["),
                    new TokenMatch(TokenType.RightBrace, "]"),
                    new TokenMatch(TokenType.LeftParenthesis, "("),
                    new TokenMatch(TokenType.RightParenthesis, ")"),
                    new TokenMatch(TokenType.GreaterThanOrEqual, ">="),
                    new TokenMatch(TokenType.GreaterThan, ">"),
                    new TokenMatch(TokenType.LessThan, "<"),
                    new TokenMatch(TokenType.LessThanOrEqual, "<="),
                    new TokenMatch(TokenType.PlusEqual, "+="),
                    new TokenMatch(TokenType.PlusPlus, "++"),
                    new TokenMatch(TokenType.Plus, "+"),
                    new TokenMatch(TokenType.MinusEqual, "-="),
                    new TokenMatch(TokenType.MinusMinus, "--"),
                    new TokenMatch(TokenType.Minus, "-"),
                    new TokenMatch(TokenType.Assignment, "="),
                    new TokenMatch(TokenType.Not, "!"),
                    new TokenMatch(TokenType.NotEqual, "!="),
                    new TokenMatch(TokenType.Mul, "*"),
                    new TokenMatch(TokenType.MulEqual, "*="),
                    new TokenMatch(TokenType.Div, "/"),
                    new TokenMatch(TokenType.DivEqual, "/="),
                    new TokenMatch(TokenType.BooleanAnd, "&&"),
                    new TokenMatch(TokenType.BooleanOr, "||"),
                    new TokenMatch(TokenType.BitwiseAnd, "&"),
                    new TokenMatch(TokenType.BitwiseOr, "|"),
                    new TokenMatch(TokenType.BitwiseAndEqual, "&="),
                    new TokenMatch(TokenType.BitwiseOrEqual, "|="),
                    new TokenMatch(TokenType.ModEqual, "%="),
                    new TokenMatch(TokenType.Mod, "%"),
                    new TokenMatch(TokenType.BitwiseXorEqual, "^="),
                    new TokenMatch(TokenType.BitwiseXor, "^"),
                    new TokenMatch(TokenType.DoubleQuestion, "??"),
                    new TokenMatch(TokenType.Question, "?"),
                    new TokenMatch(TokenType.Equal, "=="),
                    new TokenMatch(TokenType.BitShiftLeft, "<<"),
                    new TokenMatch(TokenType.BitShiftRight, ">>"),
                    new TokenMatch(TokenType.Dot, "."),
                    new TokenMatch(TokenType.Comma, ","),
                    new TokenMatch(TokenType.Semicolon, ";"),
                    new TokenMatch(TokenType.Colon, ":"),
                    new TokenMatch(TokenType.Arrow, "->"),
                    new TokenMatch(TokenType.FatArrow, "=>"),
                    new TokenMatch(TokenType.CharLiteral, "'"),
                }
            });

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var tokens = lexer.Tokenize(string.Join(Environment.NewLine, content)).ToList();

            stopwatch.Stop();

            Console.WriteLine($"Lexer took {stopwatch.ElapsedMilliseconds}ms to generate {tokens.Count} tokens");
            Console.WriteLine();

            foreach (var token in tokens)
                Console.WriteLine(token.ToString());

            if (lexer.ErrorSink.HasErrors)
            {
                Console.WriteLine("---------- ERRORS: ----------");

                foreach(var error in lexer.ErrorSink)
                    Console.WriteLine($"Severity: {error.Severity}, Message: {error.Message}, Location: (Start LineNo) {error.FilePart.Start.Line} (Start Col) {error.FilePart.Start.Column}, (End LineNo) {error.FilePart.End.Line}, (End Col) {error.FilePart.End.Column}. Value: {string.Join(" ", error.FilePart.Lines)}");
            }

            Console.WriteLine("-----------------------------");

            foreach (var token in tokens)
                PrintToken(token);

            Console.ReadLine();
        }

        public static void PrintToken(IToken token)
        {
            Console.ResetColor();

            switch(token.TokenType)
            {
                case TokenType.Keyword:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case TokenType.LineComment:
                case TokenType.BlockComment:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case TokenType.IntegerLiteral:
                case TokenType.RealLiteral:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case TokenType.StringLiteral:
                case TokenType.CharLiteral:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            Console.Write(token.Value);
        }
    }
}
