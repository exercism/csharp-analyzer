using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analyzers;
using Serilog;
using static Exercism.Analyzers.CSharp.Analyzers.DefaultComments;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalyzer
    {
        public static async Task<SolutionAnalysis> Analyze(Solution solution)
        {
            Log.Information("Compiling exercise {Exercise}.", solution.Slug);
            var compiledSolution = await SolutionCompiler.Compile(solution);
            if (compiledSolution == null)
                return null;

            var solutionAnalysis = AnalyzeCompiledSolution(compiledSolution);
            Log.Information("Analyzed exercise {Exercise} with status {Status} and comments {Comments}.", solution.Slug, solutionAnalysis.Result.Status, solutionAnalysis.Result.Comments);

            return solutionAnalysis;
        }

        private static SolutionAnalysis AnalyzeCompiledSolution(CompiledSolution compiledSolution)
        {
            if (compiledSolution.HasErrors())
                return compiledSolution.DisapproveWithComment(HasCompileErrors);

            switch (compiledSolution.Solution.Slug)
            {
                case Exercises.TwoFer: return TwoFerAnalyzer.Analyze(compiledSolution);
                case Exercises.Gigasecond: return GigasecondAnalyzer.Analyze(compiledSolution);
                case Exercises.Leap: return LeapAnalyzer.Analyze(compiledSolution);
                default: return DefaultAnalyzer.Analyze(compiledSolution);
            }
        }
    }
}