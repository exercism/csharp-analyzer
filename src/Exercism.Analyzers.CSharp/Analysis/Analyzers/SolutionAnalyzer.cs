using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.EntityFrameworkCore.Internal;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal abstract class SolutionAnalyzer
    {
        public async Task<Diagnostic[]> Analyze(CompiledSolution compiledSolution)
        {
            foreach (var rule in GetRules())
            {
                var diagnostics = await rule.Verify(compiledSolution);

                // TODO: consider if an error should halt analysis and if multiple diagnostics should be returned
                if (EnumerableExtensions.Any(diagnostics))
                    return diagnostics;
            }

            return Array.Empty<Diagnostic>();
        }

        private IEnumerable<Rule> GetRules() => GetDefaultRules().Concat(GetNonDefaultRules());

        private static IEnumerable<Rule> GetDefaultRules()
        {
            yield return new CompilesWithoutErrorsRule();
            yield return new AllTestsPassRule();
            yield return new ConvertToExpressionBodiedMemberRule();
        }

        protected virtual IEnumerable<Rule> GetNonDefaultRules() => Enumerable.Empty<Rule>();
    }
}