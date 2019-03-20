namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class UnsupportedExerciseAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            parsedSolution.ReferToMentor();
    }
}