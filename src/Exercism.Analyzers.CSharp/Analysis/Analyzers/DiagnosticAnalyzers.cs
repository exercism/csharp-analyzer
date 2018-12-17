using System.Collections.Generic;
using System.Collections.Immutable;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class DiagnosticAnalyzers
    {
        public static ImmutableArray<DiagnosticAnalyzer> ForSolution(Solution solution)
            => GetAnalyzers(solution).ToImmutableArray();

        private static IEnumerable<DiagnosticAnalyzer> GetAnalyzers(Solution solution)
        {
            yield return new CompilesSuccessfullyAnalyzer();
            yield return new AllTestPassAnalyzer();
            
            if (solution.Slug == "leap")
                yield return new LeapUsesMinimumNumberOfChecksAnalyzer();
        }
    }
}