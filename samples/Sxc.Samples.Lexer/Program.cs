using System;
using System.IO;

namespace Sxc.Samples.Lexer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var content = File.ReadAllLines("./Input.sx");

            var lexer = new Sxc.Lexer.SxcLexer();
            var tokens = lexer.Tokenize(string.Join(Environment.NewLine, content));

            foreach (var token in tokens)
                Console.WriteLine(token.ToString());

            Console.ReadLine();
        }
    }
}
