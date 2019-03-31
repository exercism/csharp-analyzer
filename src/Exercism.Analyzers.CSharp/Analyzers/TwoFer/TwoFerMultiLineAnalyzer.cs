namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerMultiLineAnalyzer
    {
        public static SolutionAnalysis AnalyzeMultiLine(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeParameterAssignment() ??
            twoFerSolution.AnalyzeVariableAssignment();
    }
}