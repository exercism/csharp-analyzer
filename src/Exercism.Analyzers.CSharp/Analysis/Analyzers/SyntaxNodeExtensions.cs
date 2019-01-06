using System.IO;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class SyntaxNodeExtensions
    {
        public static bool SkipAnalysis(this SyntaxNode context) =>
            context.IsTestFile() || context.IsProgramFile();

        private static bool IsTestFile(this SyntaxNode context) =>
            context.GetFileName().EndsWith("Test.cs");

        private static bool IsProgramFile(this SyntaxNode context) =>
            context.GetFileName().EndsWith("Program.cs");

        private static string GetFileName(this SyntaxNode context) =>
            Path.GetFileName(context.SyntaxTree.FilePath);
    }
}