using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Exercism.Analyzers.CSharp.Syntax;

using Humanizer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Exercism.Analyzers.CSharp;

internal static class Compiler
{
    public static CSharpCompilation Compile(SyntaxTree syntaxTree) =>
        CSharpCompilation.Create(AssemblyName(), new[] { syntaxTree }, References(), CompilationOptions());

    private static string AssemblyName() => Guid.NewGuid().ToString("N");

    private static string GetSolutionName(Options options) =>
        options.Slug.Dehumanize().Pascalize();

    private static SyntaxTree? Parse(Options options)
    {
        var implementationFile = EnumerateSolutionFiles(options);
        if (!implementationFile.Exists)
            return null;
        
        using var fileStream = implementationFile.OpenRead();
        var sourceText = SourceText.From(fileStream);
        return CSharpSyntaxTree.ParseText(sourceText);
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
        var configJson = JsonObject.Parse(configJsonFile);
        return configJson["files"]["test"].GetValue<string[]>();
    }

    private static CSharpCompilationOptions CompilationOptions() =>
        new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Debug);

    private static IEnumerable<PortableExecutableReference> References() =>
        ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))!.Split(Path.PathSeparator)
        .Select(p => MetadataReference.CreateFromFile(p));
}