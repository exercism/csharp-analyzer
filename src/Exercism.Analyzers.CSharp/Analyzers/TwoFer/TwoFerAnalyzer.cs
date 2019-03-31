namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new TwoFerSolution(parsedSolution));

        private static SolutionAnalysis Analyze(TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeError() ??
            twoFerSolution.AnalyzeSingleLine() ??
            twoFerSolution.AnalyzeMultiLine() ??
            twoFerSolution.ReferToMentor();
    }
}