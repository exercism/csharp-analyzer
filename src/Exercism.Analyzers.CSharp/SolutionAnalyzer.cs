using System.IO;
using Exercism.Analyzers.CSharp.Analyzers;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution)
        {
            Log.Information("Analyzing exercise {Exercise}.", solution.Slug);
            
            var implementation = solution.ToSolutionImplementation();
            if (implementation == null)
                return null;

            var solutionAnalysis = AnalyzedSolutionImplementation(solution, implementation);
            Log.Information("Analyzed exercise {Exercise} with status {Status} and comments {Comments}.", solution.Slug, solutionAnalysis.Result.Status, solutionAnalysis.Result.Comments);
            
            return solutionAnalysis;
        }

        private static SolutionAnalysis AnalyzedSolutionImplementation(Solution solution, SolutionImplementation implementation)
        {
            if (implementation.HasErrors())
                return implementation.DisapproveWithComment("Has errors");

            switch (solution.Slug)
            {
                case Exercises.TwoFer: return TwoFerAnalyzer.Analyze(implementation);
                case Exercises.Gigasecond: return GigasecondAnalyzer.Analyze(implementation);
                case Exercises.Leap: return LeapAnalyzer.Analyze(implementation);
                default: return DefaultAnalyzer.Analyze(implementation);
            }
        }

        private static SolutionImplementation ToSolutionImplementation(this Solution solution)
        {
            if (!File.Exists(solution.Paths.ImplementationFilePath))
            {
                Log.Error("Implementation file {File} does not exist.", solution.Paths.ImplementationFilePath);
                return null;
            }
            
            var implementationCode = File.ReadAllText(solution.Paths.ImplementationFilePath);
            var implementationSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(implementationCode);

            return new SolutionImplementation(solution, new Implementation(implementationSyntaxNode));
        }
    }
}