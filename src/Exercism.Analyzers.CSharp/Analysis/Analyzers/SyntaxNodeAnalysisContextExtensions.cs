using System.IO;
using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class SyntaxNodeAnalysisContextExtensions
    {
        public static bool SkipAnalysis(this SyntaxNodeAnalysisContext context) =>
            context.IsTestFile() || context.IsProgramFile() || context.HasCompilationErrors();

        private static bool IsTestFile(this SyntaxNodeAnalysisContext context) =>
            context.GetFileName().EndsWith("Test.cs");

        private static bool IsProgramFile(this SyntaxNodeAnalysisContext context) =>
            context.GetFileName().EndsWith("Program.cs");

        private static string GetFileName(this SyntaxNodeAnalysisContext context) =>
            Path.GetFileName(context.Node.SyntaxTree.FilePath);

        private static bool HasCompilationErrors(this SyntaxNodeAnalysisContext context) =>
            context.Compilation.HasErrors();
    }
}