using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Sx.Compiler.Abstractions;
using Sx.Compiler.Parser.Semantics;
using Sx.Compiler.Parser.Syntax;
using Sx.Lexer;

namespace Sx.Compiler.Samples.Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sx").Select(f => new SourceFile(f, File.ReadAllText(f)));
            var parser = new Compiler.Parser.SyntaxParser();

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var compilationUnit = parser.Parse(files);

            stopwatch.Stop();

            Console.WriteLine($"Parser took {stopwatch.ElapsedMilliseconds}ms to generate AST from {files.Sum(f => f.Lines.Count())} lines");
            Console.WriteLine();

            Console.WriteLine($"AST:");
            Console.WriteLine();

            var astPrinter = new AstPrintVisitor();

            if (compilationUnit != null)
                foreach (var node in compilationUnit.Asts)
                    astPrinter.Visit(node);

            if (parser.ErrorSink.HasErrors)
            {
                Console.WriteLine("----------- ERRORS: -----------");

                foreach (var error in parser.ErrorSink.Where(x => x.Severity == Severity.Error))
                    Console.WriteLine($"Message: {error.Message}. Location: (Start LineNo) {error.FilePart.Start.Line} (Start Col) {error.FilePart.Start.Column}, (End LineNo) {error.FilePart.End.Line}, (End Col) {error.FilePart.End.Column}. Value: {string.Join(" ", error.FilePart.Lines ?? new[] { string.Empty })}");

                Console.WriteLine();
                Console.WriteLine("---------- WARNINGS: ----------");
                Console.WriteLine();

                foreach (var error in parser.ErrorSink.Where(x => x.Severity == Severity.Warning))
                    Console.WriteLine($"Message: {error.Message}. Location: (Start LineNo) {error.FilePart.Start.Line} (Start Col) {error.FilePart.Start.Column}, (End LineNo) {error.FilePart.End.Line}, (End Col) {error.FilePart.End.Column}. Value: {string.Join(" ", error.FilePart.Lines ?? new[] { string.Empty })}");

                Console.WriteLine();
                Console.WriteLine("------------ INFO: ------------");
                Console.WriteLine();

                foreach (var error in parser.ErrorSink.Where(x => x.Severity == Severity.Message))
                    Console.WriteLine($"Message: {error.Message}. Location: (Start LineNo) {error.FilePart.Start.Line} (Start Col) {error.FilePart.Start.Column}, (End LineNo) {error.FilePart.End.Line}, (End Col) {error.FilePart.End.Column}. Value: {string.Join(" ", error.FilePart.Lines ?? new[] { string.Empty })}");

                Console.WriteLine();
            }

            var analysis = new SemanticAnalyzer(parser.ErrorSink, compilationUnit);

            Console.ReadLine();
        }
    }
}
