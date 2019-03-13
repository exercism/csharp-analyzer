using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Simplification;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionCompiler
    {
        public static async Task<CompiledSolution> Compile(Solution solution)
        {
            if (!File.Exists(solution.Paths.ImplementationFilePath))
            {
                Log.Error("Implementation file {File} does not exist.", solution.Paths.ImplementationFilePath);
                return null;
            }

            var document = await CreateDocument(solution).Reduce();
            var compilation = await document.Project.GetCompilationAsync();
            var syntaxRoot = await document.GetSyntaxRootAsync();

            var implementation = new Implementation(syntaxRoot, document, compilation);
            return new CompiledSolution(solution, implementation);
        }

        private static Document CreateDocument(Solution solution) =>
            new AdhocWorkspace()
                .AddProject(solution.Name, LanguageNames.CSharp)
                .WithMetadataReferences(GetMetadataReferences())
                .WithCompilationOptions(GetCompilationOptions())
                .AddDocument(Path.GetFileName(solution.Paths.ImplementationFilePath),
                    File.ReadAllText(solution.Paths.ImplementationFilePath));

        private static CSharpCompilationOptions GetCompilationOptions() =>
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        private static IEnumerable<PortableExecutableReference> GetMetadataReferences() =>
            AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")
                .ToString()
                .Split(":")
                .Select(metadataFilePath => MetadataReference.CreateFromFile(metadataFilePath));

        private static async Task<Document> Reduce(this Document originalDocument)
        {
            var syntaxRoot = await originalDocument.GetSyntaxRootAsync();

            return await Simplifier.ReduceAsync(
                originalDocument.WithSyntaxRoot(
                    syntaxRoot
                        .NormalizeWhitespace()
                        .WithAdditionalAnnotations(Simplifier.Annotation)));
        }
    }
}