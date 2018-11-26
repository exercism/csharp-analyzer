using Humanizer;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class Solution
    {
        public string Id { get; }
        public string Slug { get; }
        public string Name { get; }

        public Solution(string id, string slug)
            => (Slug, Id, Name) = (slug, id, slug.Dehumanize().Pascalize());
    }
}