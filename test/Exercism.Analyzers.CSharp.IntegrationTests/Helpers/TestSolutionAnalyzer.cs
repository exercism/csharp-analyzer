using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(string slug, string name, string code) =>
            Run(new TestSolution(slug, name), code);

        public static TestSolutionAnalysisRun Run(TestSolution testSolution, string code)
        {
            testSolution.CreateFiles(code);

            Program.Main(new[] {testSolution.Slug, testSolution.Directory});

            return CreateTestSolutionAnalyisRun(testSolution);
        }

        private static TestSolutionAnalysisRun CreateTestSolutionAnalyisRun(TestSolution solution)
        {
            using (var fileReader = File.OpenText(Path.Combine(solution.Directory, "analysis.json")))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var jsonAnalysisResult = JToken.ReadFrom(jsonReader);

                // We read each JSON property by key to verify that the format is correct.
                // Automatically deserializing could possible use different keys. 
                var status = jsonAnalysisResult["status"].ToObject<string>();
                var comments = jsonAnalysisResult["comments"].ToObject<string[]>();

                return new TestSolutionAnalysisRun(status, comments);
            }
        }
    }
}