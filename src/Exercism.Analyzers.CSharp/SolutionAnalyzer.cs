using Exercism.Analyzers.CSharp.Analyzers;
using Humanizer;
using Serilog;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static SolutionAnalysis Analyze(Options options)
        {
            var solution = CreateSolution(options);
            
            Log.Information("Compiling exercise {Exercise}.", solution.Slug);
            var parsedSolution = SolutionParser.Parse(solution);
            if (parsedSolution == null)
                return new SolutionAnalysis(solution, new SolutionAnalysisResult(SolutionStatus.ReferToMentor));

            var solutionAnalysis = AnalyzeParsedSolution(parsedSolution);
            Log.Information("Analyzed exercise {Exercise} with status {Status} and comments {Comments}.", solution.Slug, solutionAnalysis.Result.Status, solutionAnalysis.Result.Comments);

            return solutionAnalysis;
        }

        private static SolutionAnalysis AnalyzeParsedSolution(ParsedSolution parsedSolution)
        {
            if (parsedSolution.SyntaxRoot.HasErrorDiagnostics())
                return parsedSolution.DisapproveWithComment(HasCompileErrors);

            switch (parsedSolution.Solution.Slug)
            {
                case Exercises.TwoFer: return TwoFerAnalyzer.Analyze(parsedSolution);
                case Exercises.Gigasecond: return GigasecondAnalyzer.Analyze(parsedSolution);
                case Exercises.Leap: return LeapAnalyzer.Analyze(parsedSolution);
                default: return DefaultAnalyzer.Analyze(parsedSolution);
            }
        }

        private static Solution CreateSolution(Options options) =>
            new Solution(options.Slug,options.Slug.Dehumanize().Pascalize(), options.Directory);
    }
}