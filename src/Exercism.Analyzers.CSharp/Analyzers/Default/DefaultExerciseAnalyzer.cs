using Exercism.Analyzers.CSharp.Analyzers.Shared;

namespace Exercism.Analyzers.CSharp.Analyzers.Default
{
    internal class DefaultExerciseAnalyzer : SharedAnalyzer<DefaultSolution>
    {
        protected override SolutionAnalysis DisapproveWhenInvalid(DefaultSolution solution) =>
            solution.ContinueAnalysis();

        protected override SolutionAnalysis ApproveWhenValid(DefaultSolution solution) =>
            solution.ContinueAnalysis();
    }
}