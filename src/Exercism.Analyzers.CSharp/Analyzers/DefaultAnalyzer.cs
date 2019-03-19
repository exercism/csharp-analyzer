namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class DefaultAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            parsedSolution.ReferToMentor();
    }
}