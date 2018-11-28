using System.Collections.Generic;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules;
using Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules.Leap;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal class LeapSolutionAnalyzer : SolutionAnalyzer
    {
        protected override IEnumerable<Rule> GetNonDefaultRules()
        {
            yield return new UseMinimumNumberOfChecksInIsLeapYearMethodRule();
        }
    }
}