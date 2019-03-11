using Serilog;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    public static class DefaultAnalyzer
    {
        public static AnalyzedSolution Analyze(ImplementedSolution implementedSolution)
        {
            Log.Information("Analysing {Exercise} using {Analyzer}", 
                implementedSolution.Solution.Exercise, nameof(DefaultAnalyzer));

            return new AnalyzedSolution(implementedSolution.Solution, SolutionStatus.ReferToMentor);
        }
    }
}