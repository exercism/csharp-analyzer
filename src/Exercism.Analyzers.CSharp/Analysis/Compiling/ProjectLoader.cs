using System.IO;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Compiling
{
    internal static class ProjectLoader
    {
        private static readonly AnalyzerManager AnalyzerManager = new AnalyzerManager();

        public static Project LoadFromFile(FileInfo projectFile)
        {
            var project = AnalyzerManager.GetProject(projectFile.FullName);
            return project.AddToWorkspace(new AdhocWorkspace());
        }
    }
}