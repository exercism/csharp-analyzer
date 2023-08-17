namespace Exercism.Analyzers.CSharp.Analyzers.Default;

internal class DefaultAnalyzer : ExerciseAnalyzer<DefaultSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(DefaultSolution solution) => solution.Analysis;
}