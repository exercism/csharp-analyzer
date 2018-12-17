using System.IO;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class DownloadedSolution
    {
        public Solution Solution { get; }
        public FileInfo ProjectFile { get; }
        
        public DownloadedSolution(Solution solution, FileInfo projectFile)
            => (Solution, ProjectFile) = (solution, projectFile);
    }
}