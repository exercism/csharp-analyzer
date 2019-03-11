using System.IO;
using Exercism.Analyzers.CSharp.Analyzers;

namespace Exercism.Analyzers.CSharp
{
    public static class SolutionAnalyzer
    {
        public static AnalyzedSolution Analyze(Solution solution)
        {
            var implementedSolution = solution.ToImplementedSolution();

            switch (solution.Exercise)
            {
                case Exercises.Gigasecond: return GigasecondAnalyzer.Analyze(implementedSolution);
                case Exercises.Leap: return LeapAnalyzer.Analyze(implementedSolution);
                default: return DefaultAnalyzer.Analyze(implementedSolution);
            }
        }

        private static ImplementedSolution ToImplementedSolution(this Solution solution)
        {
            var implementationCode = File.ReadAllText(solution.Paths.ImplementationFilePath);
            var implementationSyntaxNode = SyntaxNodeParser.ParseNormalizedRoot(implementationCode);

            return new ImplementedSolution(solution, implementationSyntaxNode);
        }
    }
}