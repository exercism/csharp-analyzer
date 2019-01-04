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
    internal static class SolutionAnalyzers
    {   
        public static ImmutableArray<DiagnosticAnalyzer> Create(Solution solution) =>
            SharedAnalyzers.All
                .Concat(ExerciseAnalyzers(solution.Exercise))
                .ToImmutableArray();

        private static IEnumerable<DiagnosticAnalyzer> ExerciseAnalyzers(Exercise exercise)
        {
            if (exercise.Equals(Exercise.Leap))
                return LeapAnalyzers.All;
            if (exercise.Equals(Exercise.Gigasecond))
                return GigasecondAnalyzers.All;

            return Array.Empty<DiagnosticAnalyzer>();
        }
    }
}