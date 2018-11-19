using System.Collections.Generic;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public class LeapSolutionAnalyzer : SolutionAnalyzer
    {
        protected override IEnumerable<Rule> GetRules()
        {
            yield return new NoFailingTestsRule();
        }
    }
}