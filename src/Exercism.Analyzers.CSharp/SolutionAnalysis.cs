namespace Exercism.Analyzers.CSharp
{
    internal class SolutionAnalysis
    {
        public Solution Solution { get; }
        public SolutionAnalysisResult Result { get; }

        public SolutionAnalysis(Solution solution, SolutionAnalysisResult result) =>
            (Solution, Result) = (solution, result);
    }
}