using System;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Exercism.Analyzers.CSharp.Analyzers.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class SharedAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.HasCompileErrors())
                return parsedSolution.DisapproveWithComment(FixCompileErrors);

            if (parsedSolution.HasMainMethod())
                return parsedSolution.DisapproveWithComment(RemoveMainMethod);

            if (parsedSolution.ThrowsNotImplementedException())
                return parsedSolution.DisapproveWithComment(RemoveThrowNotImplementedException);

            if (parsedSolution.WritesToConsole())
                return parsedSolution.DisapproveWithComment(DontWriteToConsole);

            return null;
        }

        private static bool HasCompileErrors(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

        private static bool HasMainMethod(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.GetClassMethod("Program", "Main") != null;

        private static bool ThrowsNotImplementedException(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.ThrowsExceptionOfType<NotImplementedException>();

        private static bool WritesToConsole(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot.InvokesMethod(SyntaxFactory.IdentifierName("Console"), SyntaxFactory.IdentifierName("WriteLine")) ||
            parsedSolution.SyntaxRoot.InvokesMethod(SyntaxFactory.IdentifierName("Console"), SyntaxFactory.IdentifierName("ReadLine"));
    }
}