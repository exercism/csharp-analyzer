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

internal static class Loader
{
    public static async Task<Solution> Load(Options options)
    {
        var syntaxTrees = await Parser.ParseSyntaxTrees(EnumerateSolutionFiles(options));
        var compilation = Compiler.Compile(syntaxTrees);
        return new Solution(options.Slug, compilation);
    }

    private static IEnumerable<FileInfo> EnumerateSolutionFiles(Options options)
    {
        var testFiles = GetTestFiles(options);

        return Directory.EnumerateFiles(options.InputDirectory, "*.cs")
            .Where(sourceFile => !testFiles.Contains(sourceFile))
            .Select(sourceFile => new FileInfo(sourceFile));
    }

    private static string[] GetTestFiles(Options options)
    {
        var configJsonFilePath = Path.Combine(options.InputDirectory, ".meta", "config.json");
        using var configJsonFile = new FileStream(configJsonFilePath, FileMode.Open);
        var configJson = JsonNode.Parse(configJsonFile);
        return configJson["files"]["test"].Deserialize<string[]>();
    }

    private static class Parser
    {
        public static async Task<IEnumerable<SyntaxTree>> ParseSyntaxTrees(IEnumerable<FileInfo> files) =>
            await Task.WhenAll(files.Select(ParseSyntaxTree).ToArray());

        private static async Task<SyntaxTree> ParseSyntaxTree(FileInfo implementationFile)
        {
            var text = await File.ReadAllTextAsync(implementationFile.FullName);
            var sourceText = SourceText.From(text);
            return CSharpSyntaxTree.ParseText(sourceText);
        }
    }

    private static class Compiler
    {
        public static CSharpCompilation Compile(IEnumerable<SyntaxTree> syntaxTrees) =>
            CSharpCompilation.Create(AssemblyName(), syntaxTrees, References(), CompilationOptions());

        private static string AssemblyName() => Guid.NewGuid().ToString("N");

        private static CSharpCompilationOptions CompilationOptions() =>
            new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug);

        private static IEnumerable<PortableExecutableReference> References() =>
            ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))!.Split(Path.PathSeparator)
            .Select(p => MetadataReference.CreateFromFile(p));
    }
}
