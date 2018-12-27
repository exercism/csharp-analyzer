using Humanizer;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions
{   
    public readonly struct Exercise
    {
        public static readonly Exercise Leap = new Exercise("leap");
        public static readonly Exercise Gigasecond = new Exercise("gigasecond");

        public readonly string Slug;
        public readonly string Name;

        public Exercise(string slug) => (Slug, Name) = (slug, slug.Dehumanize().Pascalize());
    }
}