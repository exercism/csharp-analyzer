using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
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

            var document = solution.ToDocument();
            return new ParsedSolution(solution, document.GetReducedSyntaxRoot());
        }

        private static Document ToDocument(this Solution solution)
        {
            var workspace = new AdhocWorkspace();

            var sourceText = SourceText.From(File.ReadAllText(solution.Paths.ImplementationFilePath));
            var project = workspace.AddProject(solution.Name, LanguageNames.CSharp);
            return project.AddDocument($"{solution.Name}.cs", sourceText);
        }
    }
}