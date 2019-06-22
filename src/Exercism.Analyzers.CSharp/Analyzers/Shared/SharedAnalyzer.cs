using System;
using System.IO;
using System.Linq;
using System.Text;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.HasCompileErrors())
                return parsedSolution.DisapproveWithComment(SharedComments.FixCompileErrors);

            if (parsedSolution.HasMainMethod())
                return parsedSolution.DisapproveWithComment(SharedComments.RemoveMainMethod);

            if (parsedSolution.ThrowsNotImplementedException())
                return parsedSolution.DisapproveWithComment(SharedComments.RemoveThrowNotImplementedException);

            if (parsedSolution.WritesToConsole())
                return parsedSolution.DisapproveWithComment(SharedComments.DontWriteToConsole);

            return null;
        }

        private static bool HasCompileErrors(this ParsedSolution parsedSolution)
        {
            var compilation = CSharpCompilation.Create(
                parsedSolution.Solution.Slug,
                new[] { SyntaxTree(parsedSolution.SyntaxRoot, null, "", Encoding.Default) },
                new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using (var dllStream = new MemoryStream())
            using (var pdbStream = new MemoryStream())
            {
                var emitResult = compilation.Emit(dllStream, pdbStream);
                return !emitResult.Success && emitResult.Diagnostics.Any(result => result.Severity == DiagnosticSeverity.Error);
            }
        }

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