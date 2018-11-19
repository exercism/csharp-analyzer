using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public static class SolutionAnalyzerFactory
    {
        public static SolutionAnalyzer Create(Solution solution)
        {
            switch (solution.Slug)
            {
                case "leap": return new LeapSolutionAnalyzer();
                default: return new DefaultSolutionAnalyzer();
            }
        }
    }
}