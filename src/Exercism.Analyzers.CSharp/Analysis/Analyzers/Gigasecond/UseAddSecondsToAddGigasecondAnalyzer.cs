using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Gigasecond
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class UseAddSecondsToAddGigasecondAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: DiagnosticIds.UseAddSecondsToAddGigasecondAnalyzerRuleId,
            title: "Use AddSeconds",
            messageFormat: "You could use `AddSeconds()`.",
            category: DiagnosticCategories.ApiUsage,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterOperationAction(AnalyzeInvocation, OperationKind.Invocation);
            context.RegisterOperationAction(AnalyzeBinaryOperator, OperationKind.BinaryOperator);
        }

        private static void AnalyzeInvocation(OperationAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;

            var invocationOperation = (IInvocationOperation)context.Operation;
            var targetMethod = invocationOperation.TargetMethod.ToDisplayString();

            if (targetMethod == "System.DateTime.Add(System.TimeSpan)")
                context.ReportDiagnostic(Diagnostic.Create(Rule, invocationOperation.Syntax.GetLocation(), invocationOperation.Syntax.ToString()));
        }

        private static void AnalyzeBinaryOperator(OperationAnalysisContext context)
        {
            if (context.SkipAnalysis())
                return;

            var binaryOperation = (IBinaryOperation)context.Operation;
            var targetMethod = binaryOperation.OperatorMethod.ToDisplayString();

            if (targetMethod == "System.DateTime.operator +(System.DateTime, System.TimeSpan)")
                context.ReportDiagnostic(Diagnostic.Create(Rule, binaryOperation.Syntax.GetLocation(), binaryOperation.Syntax.ToString()));
        }
    }
}