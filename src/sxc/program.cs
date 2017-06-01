using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser;
using Sx.Compiler.Parser.Semantics;
using Sx.Lexer;

namespace Sxc
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("=============================");
            Console.WriteLine();
            Console.WriteLine("   Sx-Lang Compiler v0.0.1   ");
            Console.WriteLine();
            Console.WriteLine("=============================");
            Console.WriteLine();

            // Read command line args for file / project - ALLOW GLOBBING!?!?
            var fileName = "Input.sx";

            var parser = new SyntaxParser();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var compilationUnit = parser.Parse(new SourceFile(fileName, File.ReadAllText(fileName)));



            var semantics = new SemanticAnalyzer(parser.ErrorSink, compilationUnit);

            stopWatch.Stop();

            if (semantics.ErrorSink.HasErrors)
            {
                Console.WriteLine($"Finished compilation in {stopWatch.ElapsedMilliseconds}ms with the following errors: ");
                Console.WriteLine();

                foreach (var error in parser.ErrorSink.Errors.Where(error => error.Severity == Severity.Error))
                {
                    var beginingPadding = string.Join("", error.FilePart.Start.Line.ToString().Select(x => " "));

                    Console.WriteLine($"Error: {error.Message}");
                    Console.WriteLine($" -> {error.FilePart.FileName}");
                    Console.WriteLine();
                    Console.WriteLine($"{beginingPadding} |>    ");
                    Console.WriteLine($"{error.FilePart.Start.Line} |>    {error.FilePart.Lines.FirstOrDefault()?.Trim()}");
                    Console.WriteLine($"{beginingPadding} |>    {string.Join("", Enumerable.Range(0, error.FilePart.Lines.FirstOrDefault()?.Trim().Length ?? 0).Select(x => "^"))}");
                    Console.WriteLine();

                }

                Environment.Exit(-1);
            }

            stopWatch.Stop();

            Console.WriteLine();
            Console.WriteLine($"Finished compilation in {stopWatch.ElapsedMilliseconds:n0}ms. Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}