using System.Collections.Generic;
using System.IO;
using System.Linq;
using Humanizer;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public static class TestSolutionsReader
    {
        public static IEnumerable<TestSolution> ReadAll() =>
            from exerciseDirectory in GetExerciseDirectories("Solutions")
            from solutionDirectory in GetSolutionDirectories(exerciseDirectory)
            select CreateTestSolution(exerciseDirectory, solutionDirectory);

        private static string[] GetExerciseDirectories(string solutionsDirectory) =>
            Directory.GetDirectories(solutionsDirectory);

        private static string[] GetSolutionDirectories(string exerciseDirectory) =>
            Directory.GetDirectories(exerciseDirectory);

        private static TestSolution CreateTestSolution(string exerciseDirectory, string solutionDirectory) =>
            new TestSolution(GetExerciseSlug(exerciseDirectory), GetExerciseName(exerciseDirectory), solutionDirectory);

        private static string GetExerciseSlug(string exerciseDirectory) =>
            GetExerciseName(exerciseDirectory).Kebaberize();

        private static string GetExerciseName(string exerciseDirectory) =>
            Path.GetFileName(exerciseDirectory);
    }
}