using Humanizer;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{
    public class Solution
    {
        public string Slug { get; }
        public string Uuid { get; }
        public string Name { get; }

        public Solution(string slug, string uuid)
            => (Slug, Uuid, Name) = (slug, uuid, slug.Dehumanize().Pascalize());
    }
}