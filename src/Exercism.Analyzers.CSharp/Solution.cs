using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    internal class Solution
    {
        public string Name { get; }
        public string Slug { get; }
        public SolutionPaths Paths { get; }

        public Solution(string slug, string directory)
        {
            Slug = slug;
            Name = slug.Dehumanize().Pascalize();
            Paths = new SolutionPaths(Name, directory);
        }
    }
}