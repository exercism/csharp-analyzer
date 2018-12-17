using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class CompilesSuccessfullyAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "EXERCISM0001",
            title: "Solution has compilation errors",
            messageFormat: "The solution does not compile.",
            category: "SolutionCorrectness",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
            => context.RegisterCompilationAction(AnalyzeCompilation);

        private static void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            if (context.Compilation.GetDiagnostics().Any(IsErrorDiagnostic))
                context.ReportDiagnostic(Diagnostic.Create(Rule, Location.None));
        }

        private static bool IsErrorDiagnostic(Diagnostic diagnostic) 
            => diagnostic.Severity == DiagnosticSeverity.Error;
    }
}