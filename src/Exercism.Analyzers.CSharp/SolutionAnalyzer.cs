using Exercism.Analyzers.CSharp.Analyzers;
using Humanizer;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static SolutionAnalysis Analyze(Options options)
        {
            var solution = CreateSolution(options);
            
            Log.Information("Parsing exercise {Exercise} in {Directory}.", options.Slug, options.Directory);
            var parsedSolution = SolutionParser.Parse(solution);
            if (parsedSolution == null)
                return new SolutionAnalysis(solution, new SolutionAnalysisResult(SolutionStatus.ReferToMentor));

            var solutionAnalysis = AnalyzeSolution(parsedSolution);
            Log.Information("Analyzed exercise {Exercise} with status {Status} and comments {Comments}.", solution.Slug, solutionAnalysis.Result.Status, solutionAnalysis.Result.Comments);

            return solutionAnalysis;
        }

        private static Solution CreateSolution(Options options) =>
            new Solution(options.Slug,options.Slug.Dehumanize().Pascalize(), options.Directory);

        private static SolutionAnalysis AnalyzeSolution(ParsedSolution parsedSolution) =>
            AnalyzeSharesRules(parsedSolution) ??
            AnalyzeExerciseSpecificRules(parsedSolution);

        private static SolutionAnalysis AnalyzeSharesRules(ParsedSolution parsedSolution) =>
            SharedAnalyzer.Analyze(parsedSolution);

        private static SolutionAnalysis AnalyzeExerciseSpecificRules(ParsedSolution parsedSolution)
        {
            switch (parsedSolution.Solution.Slug)
            {
                case Exercises.TwoFer:
                    return TwoFerAnalyzer.Analyze(parsedSolution);
                case Exercises.Gigasecond:
                    return GigasecondAnalyzer.Analyze(parsedSolution);
                case Exercises.Leap:
                    return LeapAnalyzer.Analyze(parsedSolution);
                default:
                    return UnsupportedExerciseAnalyzer.Analyze(parsedSolution);
            }
        }
    }
}