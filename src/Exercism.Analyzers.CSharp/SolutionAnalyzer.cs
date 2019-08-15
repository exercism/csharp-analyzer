using Exercism.Analyzers.CSharp.Analyzers.Default;
using Exercism.Analyzers.CSharp.Analyzers.Gigasecond;
using Exercism.Analyzers.CSharp.Analyzers.Leap;
using Exercism.Analyzers.CSharp.Analyzers.TwoFer;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution)
        {
            switch (solution.Slug)
            {
                case Exercises.TwoFer:
                    return new TwoFerAnalyzer().Analyze(new TwoFerSolution(solution));
                case Exercises.Gigasecond:
                    return new GigasecondAnalyzer().Analyze(new GigasecondSolution(solution));
                case Exercises.Leap:
                    return new LeapAnalyzer().Analyze(new LeapSolution(solution));
                default:
                    return new DefaultExerciseAnalyzer().Analyze(new DefaultSolution(solution));
            }
        }
    }
}