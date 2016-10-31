using System;
using System.IO;

namespace Sxc.Samples.Lexer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var content = File.ReadAllLines("./Input.sx");

            var lexer = new Sxc.Lexer.SxcLexer(new string[] 
            {
                "import",
                "module",
                "public",
                "private",
                "internal",
                "protected",
                "class",
                "interface",
                "if",
                "else",
                "switch",
                "case",
                "default",
                "break",
                "return",
                "while",
                "for",
                "foreach",
                "let",
                "true",
                "false",

            });
            var tokens = lexer.Tokenize(string.Join(Environment.NewLine, content));

            foreach (var token in tokens)
                Console.WriteLine(token.ToString());

            if (lexer.ErrorSink.HasErrors)
            {
                Console.WriteLine("---------- ERRORS: ----------");

                foreach(var error in lexer.ErrorSink)
                    Console.WriteLine($"Severity: {error.Severity}, Message: {error.Message}, Location: (Start LineNo) {error.FilePart.Start.Line} (Start Col) {error.FilePart.Start.Column}, (End LineNo) {error.FilePart.End.Line}, (End Col) {error.FilePart.End.Column}. Value: {string.Join(" ", error.FilePart.Lines)}");
            }

            Console.ReadLine();
        }
    }
}
