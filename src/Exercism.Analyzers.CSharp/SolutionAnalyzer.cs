using Exercism.Analyzers.CSharp.Analyzers.Default;
using Exercism.Analyzers.CSharp.Analyzers.Gigasecond;
using Exercism.Analyzers.CSharp.Analyzers.Leap;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.TwoFer;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution) =>
            AnalyzeSharedRules(solution) ??
            AnalyzeExerciseSpecificRules(solution);

        private static SolutionAnalysis AnalyzeSharedRules(Solution solution) =>
            SharedAnalyzer.Analyze(solution);

        private static SolutionAnalysis AnalyzeExerciseSpecificRules(Solution solution)
        {
            switch (solution.Slug)
            {
                case Exercises.TwoFer:
                    return TwoFerAnalyzer.Analyze(new TwoFerSolution(solution));
                case Exercises.Gigasecond:
                    return GigasecondAnalyzer.Analyze(new GigasecondSolution(solution));
                case Exercises.Leap:
                    return LeapAnalyzer.Analyze(new LeapSolution(solution));
                default:
                    return DefaultExerciseAnalyzer.Analyze(new DefaultSolution(solution));
            }
        }
    }
}