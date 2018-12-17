using Exercism.Analyzers.CSharp.Analysis.Compilation;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public static class SolutionLoader
    {
        public static LoadedSolution Load(DownloadedSolution downloadedSolution)
        {
            var project = ProjectLoader.LoadFromFile(downloadedSolution.ProjectFile); 
            var testsFile = project.GetDocument(downloadedSolution.TestsFile.Name);
            var implementationFile = project.GetDocument(downloadedSolution.ImplementationFile.Name);
            
            return new LoadedSolution(downloadedSolution.Solution, project, testsFile, implementationFile);
        }
    }
}