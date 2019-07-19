using System;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution)
        {
            if (solution.NoImplementationFileFound())
                return solution.ReferToMentor();
            
            if (solution.HasCompileErrors())
                solution.AddComment(FixCompileErrors);

            if (solution.HasMainMethod())
                solution.AddComment(RemoveMainMethod);

            if (solution.ThrowsNotImplementedException())
                solution.AddComment(RemoveThrowNotImplementedException);

            if (solution.WritesToConsole())
                solution.AddComment(DoNotWriteToConsole);

            return solution.HasComments()
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }

        private static bool NoImplementationFileFound(this Solution solution) =>
            solution.SyntaxRoot == null;

        private static bool HasCompileErrors(this Solution solution) =>
            solution.SyntaxRoot.GetDiagnostics().Any(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error);

        private static bool HasMainMethod(this Solution solution) =>
            solution.SyntaxRoot.GetClassMethod("Program", "Main") != null;

        private static bool ThrowsNotImplementedException(this Solution solution) =>
            solution.SyntaxRoot.ThrowsExceptionOfType<NotImplementedException>();

        private static bool WritesToConsole(this Solution solution) =>
            solution.SyntaxRoot.InvokesMethod("Console.Write") ||
            solution.SyntaxRoot.InvokesMethod("Console.WriteAsync") ||
            solution.SyntaxRoot.InvokesMethod("Console.WriteLine") ||
            solution.SyntaxRoot.InvokesMethod("Console.WriteLineAsync") ||
            solution.SyntaxRoot.InvokesMethod("Console.Out.Write") ||
            solution.SyntaxRoot.InvokesMethod("Console.Out.WriteAsync") ||
            solution.SyntaxRoot.InvokesMethod("Console.Out.WriteLine") ||
            solution.SyntaxRoot.InvokesMethod("Console.Out.WriteLineAsync") ||
            solution.SyntaxRoot.InvokesMethod("Console.Error.Write") ||
            solution.SyntaxRoot.InvokesMethod("Console.Error.WriteAsync") ||
            solution.SyntaxRoot.InvokesMethod("Console.Error.WriteLine") ||
            solution.SyntaxRoot.InvokesMethod("Console.Error.WriteLineAsync");

    }
}