using System.Collections.Generic;
using System.Collections.Immutable;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class SolutionAnalyzers
    {
        private static readonly DiagnosticAnalyzer[] SharedAnalyzers = 
        {
            new CompilesSuccessfullyAnalyzer(),
            new AllTestPassAnalyzer(),
            new UseExpressionBodiedMemberAnalyzer()
        };
        
        public static ImmutableArray<DiagnosticAnalyzer> Create(Solution solution)
        {
            var analyzers = new List<DiagnosticAnalyzer>(SharedAnalyzers);

            if (solution.Slug == "leap")
                analyzers.Add(new LeapUsesMinimumNumberOfChecksAnalyzer());

            return analyzers.ToImmutableArray();
        }
    }
}