using System.Collections.Generic;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public static class SolutionAnalyzers
    {
        public static IEnumerable<SolutionAnalyzer> ForSolution(Solution solution)
        {
            yield return new FailingTestsSolutionAnalyzer();
//            switch (exercise.Slug)
//            {
//                case "leap":
//                    
//            }
        }
    }
}