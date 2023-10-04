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

internal record Submission(string Slug, Compilation Compilation)
{
    public bool HasCompilationErrors =>
        Compilation.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
}

internal static class Loader
{
    public static async Task<Submission> Load(Options options)
    {
        var syntaxTrees = await Task.WhenAll(SubmissionFiles.Parse(options));
        var compilation = Compiler.Compile(syntaxTrees);
        return new Submission(options.Slug, compilation);
    }

    private static class SubmissionFiles
    {
        public static IEnumerable<Task<SyntaxTree>> Parse(Options options)
        {
            var nonSubmissionFiles = NonSubmissionFiles(options);

            return Directory.EnumerateFiles(options.InputDirectory, "*.cs")
                .Where(sourceFile => !nonSubmissionFiles.Contains(sourceFile))
                .Select(async sourceFile =>
                {
                    var source = await File.ReadAllTextAsync(sourceFile);
                    return CSharpSyntaxTree.ParseText(SourceText.From(source));
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
        public static Compilation Compile(IEnumerable<SyntaxTree> syntaxTrees) =>
            CSharpCompilation.Create(Guid.NewGuid().ToString(), 
                syntaxTrees: syntaxTrees,
                references: References(),
                options: CompilationOptions());

        private static CSharpCompilationOptions CompilationOptions() =>
            new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug);

        private static IEnumerable<MetadataReference> References() =>
            ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))!.Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
    }
}