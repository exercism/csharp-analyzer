namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class DefaultExerciseAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution) =>
            solution.ReferToMentor();
    }
}