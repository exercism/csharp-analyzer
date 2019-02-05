using System.IO;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class SyntaxTreeExtensions
    {
        public static bool SkipAnalysis(this SyntaxTree syntaxTree) =>
            syntaxTree.IsTestFile() || syntaxTree.IsProgramFile();

        private static bool IsTestFile(this SyntaxTree syntaxTree) =>
            syntaxTree.GetFileName().EndsWith("Test.cs");

        private static bool IsProgramFile(this SyntaxTree syntaxTree) =>
            syntaxTree.GetFileName() == "Program.cs";

        public static bool IsImplementationFile(this SyntaxTree syntaxTree, in Exercise exercise) =>
            syntaxTree.GetFileName() == $"{exercise.Name}.cs";

        private static string GetFileName(this SyntaxTree syntaxTree) =>
            Path.GetFileName(syntaxTree.FilePath);
    }
}