using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Exercism.Analyzers.CSharp.Syntax;

using Humanizer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Exercism.Analyzers.CSharp;

internal static class SolutionLoader
{
    public static Solution Load(Options options)
    {
        var syntaxTree = Parse(options);
        return new Solution(options.Slug, GetSolutionName(options), syntaxTree, Compile(syntaxTree));
    }

    private static Compilation Compile(SyntaxTree syntaxTree) =>
        CSharpCompilation.Create(AssemblyName(), new[] { syntaxTree }, References(), CompilationOptions());

    private static string AssemblyName() => Guid.NewGuid().ToString("N");

    private static string GetSolutionName(Options options) =>
        options.Slug.Dehumanize().Pascalize();

    private static SyntaxTree? Parse(Options options)
    {
        var implementationFile = GetImplementationFile(options);
        if (!implementationFile.Exists)
            return null;
        
        using var fileStream = implementationFile.OpenRead();
        var sourceText = SourceText.From(fileStream);
        return CSharpSyntaxTree.ParseText(sourceText);
    }

    private static FileInfo GetImplementationFile(Options options)
    {
        var implementationFileName = $"{GetSolutionName(options)}.cs";
        var implementationFilePath = Path.GetFullPath(Path.Combine(options.InputDirectory, implementationFileName));

        return new FileInfo(implementationFilePath);
    }
    
    private static CSharpCompilationOptions CompilationOptions() =>
        new(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release);

    private static IEnumerable<PortableExecutableReference> References() =>
        ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES"))!.Split(Path.PathSeparator)
        .Select(p => MetadataReference.CreateFromFile(p));
}