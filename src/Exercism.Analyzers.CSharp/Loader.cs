using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Exercism.Analyzers.CSharp;

internal record Submission(string Slug, Compilation Compilation, Project Project)
{
    public bool HasCompilationErrors =>
        Compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
}

internal static class Loader
{
    private record SubmissionFile(string FilePath, SourceText SourceText);
    
    public static async Task<Submission> Load(Options options)
    {
        var workspace = new AdhocWorkspace();
        var project = workspace.AddProject(options.Slug, LanguageNames.CSharp);

        foreach (var submissionFile in await Task.WhenAll(SubmissionFiles.Enumerate(options)))
            project = project.AddDocument(submissionFile.FilePath, submissionFile.SourceText).Project;
        
        var compilation = await Compiler.Compile(project);
        return new Submission(options.Slug, compilation, project);
    }

    private static class SubmissionFiles
    {
        public static IEnumerable<Task<SubmissionFile>> Enumerate(Options options)
        {
            var nonSubmissionFiles = NonSubmissionFiles(options);

            return Directory.EnumerateFiles(options.InputDirectory, "*.cs")
                .Where(sourceFile => !nonSubmissionFiles.Contains(sourceFile))
                .Select(async sourceFile =>
                {
                    // var source = await File.ReadAllTextAsync(sourceFile);
                    var source = @"
using System.Collections.Generic;
using System.Linq;

public static class Types
{
    public static void Integrals()
    {
        int[] numbers = { 5, 10, 8, 3, 6, 12};
IEnumerable<int> numQuery2 = numbers.Where(num => num % 2 == 0).OrderBy(n => n);
    }
}
";
                        
                    return new SubmissionFile(sourceFile, SourceText.From(source));
                });
        }

        private static HashSet<string> NonSubmissionFiles(Options options)
        {
            var filesConfig = ParseConfigFiles(options);
            var nonSubmissionFiles = new HashSet<string>();
            nonSubmissionFiles.UnionWith(filesConfig?["test"]?.Deserialize<IEnumerable<string>>() ?? Enumerable.Empty<string>());
            nonSubmissionFiles.UnionWith(filesConfig?["invalidator"]?.Deserialize<IEnumerable<string>>() ?? Enumerable.Empty<string>());
            nonSubmissionFiles.UnionWith(filesConfig?["editor"]?.Deserialize<IEnumerable<string>>() ?? Enumerable.Empty<string>());
            return nonSubmissionFiles;
        }

        private static JsonNode ParseConfigFiles(Options options)
        {
            var configJsonFilePath = Path.Combine(options.InputDirectory, ".meta", "config.json");
            using var configJsonFile = new FileStream(configJsonFilePath, FileMode.Open);
            var configJson = JsonNode.Parse(configJsonFile);
            return configJson["files"];
        }
    }

    private static class Compiler
    {
        public static async Task<Compilation> Compile(Project project) =>
            await project
                .WithMetadataReferences(References())
                .WithCompilationOptions(CompilationOptions())
                .GetCompilationAsync();

        private static CSharpCompilationOptions CompilationOptions() =>
            new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug);

        private static IEnumerable<MetadataReference> References() =>
            ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))!.Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
    }
}