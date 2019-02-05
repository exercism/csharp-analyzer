using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Gigasecond;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Leap;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class AnalyzerFactory
    {
        public static ApprovalAnalyzer CreateForApproval(in Solution solution)
        {
            if (solution.Exercise.Equals(Exercise.Gigasecond))
                return GigasecondAnalyzers.ApprovalAnalyzer;

            return SharedAnalyzers.ApprovalAnalyzer;
        }
        
        public static ImmutableArray<DiagnosticAnalyzer> Create(in Solution solution) =>
            SharedAnalyzers.All
                .Concat(CreateForExercise(solution.Exercise))
                .ToImmutableArray();

        private static IEnumerable<DiagnosticAnalyzer> CreateForExercise(in Exercise exercise)
        {
            if (exercise.Equals(Exercise.Leap))
                return LeapAnalyzers.All;
            if (exercise.Equals(Exercise.Gigasecond))
                return GigasecondAnalyzers.All;

            return Array.Empty<DiagnosticAnalyzer>();
        }
    }
}