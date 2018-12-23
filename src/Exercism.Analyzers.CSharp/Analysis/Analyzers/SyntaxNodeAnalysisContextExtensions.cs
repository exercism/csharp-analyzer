using System.IO;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class SyntaxNodeAnalysisContextExtensions
    {
        public static bool SkipAnalysis(this SyntaxNodeAnalysisContext context) =>
            IsTestFile(context) || IsProgramFile(context);

        private static bool IsTestFile(SyntaxNodeAnalysisContext context) =>
            GetFileName(context).EndsWith("Test.cs");

        private static bool IsProgramFile(SyntaxNodeAnalysisContext context) =>
            GetFileName(context).EndsWith("Program.cs");

        private static string GetFileName(SyntaxNodeAnalysisContext context) =>
            Path.GetFileName(context.Node.SyntaxTree.FilePath);
    }
}