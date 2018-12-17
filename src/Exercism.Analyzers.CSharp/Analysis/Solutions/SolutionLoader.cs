using Exercism.Analyzers.CSharp.Analysis.Compiling;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public static class SolutionLoader
    {
        public static LoadedSolution Load(DownloadedSolution downloadedSolution)
        {
            var project = ProjectLoader.LoadFromFile(downloadedSolution.ProjectFile); 
            return new LoadedSolution(downloadedSolution.Solution, project);
        }
    }
}