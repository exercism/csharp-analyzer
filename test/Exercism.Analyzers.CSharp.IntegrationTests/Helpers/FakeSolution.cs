using System;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    internal readonly struct FakeSolution
    {
        public readonly string Id;
        public readonly string ImplementationFile;
        public readonly FakeExercise Exercise;
        public readonly string Category;

        public FakeSolution(string implementationFile, FakeExercise exercise, string category) =>
            (Id, ImplementationFile, Exercise, Category) = (Guid.NewGuid().ToString(), implementationFile, exercise, category);
    }
}