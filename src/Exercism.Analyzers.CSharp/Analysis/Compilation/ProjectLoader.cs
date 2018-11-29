using System.IO;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Compilation
{
    internal static class ProjectLoader
    {
        private static readonly AnalyzerManager Manager = new AnalyzerManager();
        private static readonly AdhocWorkspace Workspace = new AdhocWorkspace();
        
        public static Project LoadFromFile(FileInfo projectFile)
        {       
            Workspace.ClearSolution(); // Remove previously added projects
            
            var analyzer = Manager.GetProject(projectFile.FullName);
            return analyzer.AddToWorkspace(Workspace);
        }
    }
}