using System;
using System.IO;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser;
using Sx.Compiler.Parser.Semantics;

namespace Sxc
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("Sx-Lang Compiler");

            // Read command line args for file / project - ALLOW GLOBBING!?!?
            var fileName = "Input.sx";

            var parser = new SyntaxParser();

            var compilationUnit = parser.Parse(new Sx.Lexer.SourceFile(fileName, File.ReadAllText(fileName)));

            

            var semantics = new SemanticAnalyzer(parser.ErrorSink, compilationUnit);

            if (semantics.ErrorSink.HasErrors)
            {
                foreach (var error in parser.ErrorSink.Errors.Where(error => error.Severity == Severity.Error))
                {
                    Console.WriteLine($"Error: {error.Message}");
                    //Console.WriteLine();
                    Console.WriteLine($"{error.FilePart.Start.Line} |> {error.FilePart.Lines.FirstOrDefault()}");
                    Console.WriteLine($"{string.Join("", error.FilePart.Start.Line.ToString().Select(x => " "))} |> {string.Join("", Enumerable.Range(0, error.FilePart.End.Column - error.FilePart.Start.Column).Select(x => "^"))}");
                }

                Environment.Exit(-1);
            }

        }
    }
}