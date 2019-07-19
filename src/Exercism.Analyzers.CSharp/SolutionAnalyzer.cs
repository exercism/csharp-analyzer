using Exercism.Analyzers.CSharp.Analyzers;
using Exercism.Analyzers.CSharp.Analyzers.Gigasecond;
using Exercism.Analyzers.CSharp.Analyzers.Leap;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.TwoFer;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution) =>
            AnalyzeSharesRules(solution) ??
            AnalyzeExerciseSpecificRules(solution);

        private static SolutionAnalysis AnalyzeSharesRules(Solution solution) =>
            SharedAnalyzer.Analyze(solution);

        private static SolutionAnalysis AnalyzeExerciseSpecificRules(Solution solution)
        {
            switch (solution.Slug)
            {
                case Exercises.TwoFer:
                    return TwoFerAnalyzer.Analyze(solution);
                case Exercises.Gigasecond:
                    return GigasecondAnalyzer.Analyze(solution);
                case Exercises.Leap:
                    return LeapAnalyzer.Analyze(solution);
                default:
                    return DefaultExerciseAnalyzer.Analyze(solution);
            }
        }
    }
}