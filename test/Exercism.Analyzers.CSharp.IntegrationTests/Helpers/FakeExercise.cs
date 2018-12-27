using Humanizer;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public readonly struct FakeExercise
    {
        public static readonly FakeExercise Shared = new FakeExercise("shared");
        public static readonly FakeExercise Leap = new FakeExercise("leap");
        public static readonly FakeExercise Gigasecond = new FakeExercise("gigasecond");

        public readonly string Slug;
        public readonly string Name;

        private FakeExercise(string slug) => (Slug, Name) = (slug, slug.Dehumanize().Pascalize());
    }
}