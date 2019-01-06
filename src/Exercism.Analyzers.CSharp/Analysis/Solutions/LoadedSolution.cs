using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class LoadedSolution
    {
        public Solution Solution { get; }
        public Project Project { get; }

        public LoadedSolution(in Solution solution, Project project) => (Solution, Project) = (solution, project);
    }
}