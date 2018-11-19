using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.EntityFrameworkCore.Internal;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public abstract class SolutionAnalyzer
    {
        public async Task<Diagnostic[]> Analyze(CompiledSolution compiledSolution)
        {
            foreach (var rule in GetRules())
            {
                var diagnostics = await rule.Verify(compiledSolution);

                if (diagnostics.Any())
                    return diagnostics;
            }

            return Array.Empty<Diagnostic>();
        }

        protected abstract IEnumerable<Rule> GetRules();
    }
}