using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public abstract class AnalyzerTests
    {
        private readonly string _slug;
        private readonly string _name;

        protected AnalyzerTests(string slug, string name) =>
            (_slug, _name) = (slug, name);

        protected TestSolutionAnalysisRun Analyze(string code) =>
            TestSolutionAnalyzer.Run(_slug, _name, code);
    }
}