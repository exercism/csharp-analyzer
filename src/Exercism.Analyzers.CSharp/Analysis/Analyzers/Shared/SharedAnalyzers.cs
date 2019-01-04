using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Shared
{
    internal static class SharedAnalyzers
    {
        public static readonly DiagnosticAnalyzer[] All = 
        {
            new CompilesSuccessfullyAnalyzer(),
            new PassesAllTestAnalyzer(),
            new UseExpressionBodiedMemberAnalyzer()
        };
    }
}