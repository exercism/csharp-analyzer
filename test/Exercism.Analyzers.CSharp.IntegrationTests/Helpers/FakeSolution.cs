using System;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    internal readonly struct FakeSolution
    {
        public readonly string Id;
        public readonly string ImplementationFile;
        public readonly FakeExercise Exercise;

        public FakeSolution(string implementationFile, FakeExercise exercise) =>
            (Id, ImplementationFile, Exercise) = (Guid.NewGuid().ToString(), implementationFile, exercise);
    }
}