using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    internal class Solution
    {
        public string Name { get; }
        public string Exercise { get; }
        public SolutionPaths Paths { get; }

        public Solution(string exercise, string directory)
        {
            Exercise = exercise;
            Name = exercise.Dehumanize().Pascalize();
            Paths = new SolutionPaths(Name, directory);
        }
    }
}