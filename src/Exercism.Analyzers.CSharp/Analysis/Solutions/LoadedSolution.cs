using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class LoadedSolution
    {
        public Solution Solution { get; }

        public Project Project { get; }
        public Document TestsFile { get; }
        public Document ImplementationFile { get; }

        public LoadedSolution(Solution solution, Project project, Document testsFile, Document implementationFile)
            => (Solution, Project, TestsFile, ImplementationFile) = (solution, project, testsFile, implementationFile);
    }
}