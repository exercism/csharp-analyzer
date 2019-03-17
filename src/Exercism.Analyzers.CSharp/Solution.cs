namespace Exercism.Analyzers.CSharp
{
    internal class Solution
    {
        public string Name { get; }
        public string Slug { get; }
        public SolutionPaths Paths { get; }

        public Solution(string slug, string name, string directory) =>
            (Slug, Name, Paths) = (slug, name, new SolutionPaths(name, directory));
    }
}