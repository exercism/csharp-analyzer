using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    public class Solution
    {
        public string Name { get; }
        public string Exercise { get; }
        public string Track { get; }
        public SolutionPaths Paths { get; }

        public Solution(string exercise, string track, string directory)
        {
            Exercise = exercise;
            Name = exercise.Dehumanize().Pascalize();
            Track = track;
            Paths = new SolutionPaths(Name, directory);
        }
    }
}