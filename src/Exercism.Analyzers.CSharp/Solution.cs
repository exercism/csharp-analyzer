// using System;
// using System.Linq;
//
// using Exercism.Analyzers.CSharp.Syntax;
//
// using Microsoft.CodeAnalysis;
//
// namespace Exercism.Analyzers.CSharp;
//
// internal class Solution
// {
//     public string Name { get; }
//     public string Slug { get; }
//     public Compilation Compilation { get; }
//     public SyntaxTree SyntaxTree { get; }
//     public SyntaxNode SyntaxRoot { get; }
//     public Lazy<SemanticModel> SemanticModel { get; }
//     
//     public Solution(string slug, string name, SyntaxTree syntaxTree, Compilation compilation)
//     {
//         (Slug, Name, SyntaxTree, SyntaxRoot, Compilation) = (slug, name, syntaxTree, syntaxTree.GetRoot(), compilation);
//         SemanticModel = new Lazy<SemanticModel>(() => Compilation.GetSemanticModel(SyntaxTree));
//     }
//
//     protected Solution(Solution solution) : this(solution.Slug, solution.Slug, solution.SyntaxTree, solution.Compilation)
//     {
//     }
//
//     public bool WritesToConsole() =>
//         SyntaxRoot.InvokesMethod("Console.Write") ||
//         SyntaxRoot.InvokesMethod("Console.WriteAsync") ||
//         SyntaxRoot.InvokesMethod("Console.WriteLine") ||
//         SyntaxRoot.InvokesMethod("Console.WriteLineAsync") ||
//         SyntaxRoot.InvokesMethod("Console.Out.Write") ||
//         SyntaxRoot.InvokesMethod("Console.Out.WriteAsync") ||
//         SyntaxRoot.InvokesMethod("Console.Out.WriteLine") ||
//         SyntaxRoot.InvokesMethod("Console.Out.WriteLineAsync") ||
//         SyntaxRoot.InvokesMethod("Console.Error.Write") ||
//         SyntaxRoot.InvokesMethod("Console.Error.WriteAsync") ||
//         SyntaxRoot.InvokesMethod("Console.Error.WriteLine") ||
//         SyntaxRoot.InvokesMethod("Console.Error.WriteLineAsync");
//
//     public bool ThrowsNotImplementedException() =>
//         SyntaxRoot.ThrowsExceptionOfType<NotImplementedException>();
//
//     public bool HasMainMethod() =>
//         SyntaxRoot.GetClassMethod("Program", "Main") != null;
//
//     public bool HasCompileErrors() =>
//         SyntaxRoot.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);
//
//     public bool NoImplementationFileFound() =>
//         SyntaxRoot == null;
// }