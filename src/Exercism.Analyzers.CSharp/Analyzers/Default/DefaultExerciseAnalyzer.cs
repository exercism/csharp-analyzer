using Exercism.Analyzers.CSharp.Analyzers.Shared;

namespace Exercism.Analyzers.CSharp.Analyzers.Default;

internal class DefaultExerciseAnalyzer : SharedAnalyzer<DefaultSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(DefaultSolution solution) => null;
}