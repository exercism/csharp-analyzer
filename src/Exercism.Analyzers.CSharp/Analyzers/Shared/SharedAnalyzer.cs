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
                ? parsedSolution.DisapproveWithComment()
                : parsedSolution.ContinueAnalysis();
        }

        private static bool HasCompileErrors(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

        private static bool HasMainMethod(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.GetClassMethod("Program", "Main") != null;

        private static bool ThrowsNotImplementedException(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.ThrowsExceptionOfType<NotImplementedException>();

        private static bool WritesToConsole(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.InvokesMethod(
                SimpleMemberAccessExpression(
                    IdentifierName("Console"),
                    IdentifierName("WriteLine"))) ||
            parsedSolution.SyntaxRoot.InvokesMethod(
                SimpleMemberAccessExpression(
                    IdentifierName("Console"),
                    IdentifierName("Write")));
    }
}