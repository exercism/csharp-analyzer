using System;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.HasCompileErrors())
                parsedSolution.AddComment(SharedComments.FixCompileErrors);

            if (parsedSolution.HasMainMethod())
                parsedSolution.AddComment(SharedComments.RemoveMainMethod);

            if (parsedSolution.ThrowsNotImplementedException())
                parsedSolution.AddComment(SharedComments.RemoveThrowNotImplementedException);

            if (parsedSolution.WritesToConsole())
                parsedSolution.AddComment(SharedComments.DoNotWriteToConsole);

            return parsedSolution.HasComments()
                ? parsedSolution.Disapprove()
                : parsedSolution.ContinueAnalysis();
        }

        private static bool HasCompileErrors(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

        private static bool HasMainMethod(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.GetClassMethod("Program", "Main") != null;

        private static bool ThrowsNotImplementedException(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.ThrowsExceptionOfType<NotImplementedException>();

        private static bool WritesToConsole(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Write") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.WriteAsync") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.WriteLine") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.WriteLineAsync") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Out.Write") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Out.WriteAsync") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Out.WriteLine") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Out.WriteLineAsync") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Error.Write") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Error.WriteAsync") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Error.WriteLine") ||
            parsedSolution.SyntaxRoot.InvokesMethod("Console.Error.WriteLineAsync");

    }
}