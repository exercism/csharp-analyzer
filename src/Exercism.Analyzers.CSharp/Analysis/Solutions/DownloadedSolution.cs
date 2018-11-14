using System.IO;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class DownloadedSolution
    {
        public Solution Solution { get; }

        public FileInfo ProjectFile { get; }
        public FileInfo ImplementationFile { get; }
        public FileInfo TestsFile { get; }
        
        public DownloadedSolution(Solution solution, FileInfo projectFile, FileInfo implementationFile, FileInfo testsFile)
            => (Solution, ProjectFile, ImplementationFile, TestsFile) = (solution, projectFile, implementationFile, testsFile);
    }
}