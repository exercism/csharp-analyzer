using System.IO;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionParser
    {
        public static ParsedSolution Parse(Solution solution)
        {
            if (!File.Exists(solution.Paths.ImplementationFilePath))
            {
                Log.Error("Implementation file {File} does not exist.", solution.Paths.ImplementationFilePath);
                return null;
            }

            var syntaxRoot = SyntaxNodeParser.ParseNormalizedRoot(File.ReadAllText(solution.Paths.ImplementationFilePath));
            return new ParsedSolution(solution, syntaxRoot);
        }
    }
}