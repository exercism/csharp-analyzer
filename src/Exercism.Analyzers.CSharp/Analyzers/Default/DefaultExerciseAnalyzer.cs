namespace Exercism.Analyzers.CSharp.Analyzers.Default
{
    internal static class DefaultExerciseAnalyzer
    {
        public static SolutionAnalysis Analyze(DefaultSolution solution) =>
            solution.ReferToMentor();
    }
}