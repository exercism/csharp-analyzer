using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(string exercise, string name, string code) =>
            Run(new TestSolution(exercise, name), code);

        public static TestSolutionAnalysisRun Run(TestSolution testSolution, string code)
        {
            testSolution.CreateFiles(code);

            var returnCode = Program.Main(new[] {testSolution.Directory});
            if (returnCode > 0)
                return FailedRun(returnCode);

            return SuccessfullRun(returnCode, testSolution);
        }

        private static TestSolutionAnalysisRun FailedRun(int returnCode) =>
            new TestSolutionAnalysisRun(returnCode, TestSolutionAnalysisStatus.ReferToMentor, Array.Empty<string>());

        private static TestSolutionAnalysisRun SuccessfullRun(int returnCode, TestSolution solution)
        {
            using (var fileReader = File.OpenText(Path.Combine(solution.Directory, "analysis.json")))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var jsonAnalysisResult = JToken.ReadFrom(jsonReader);

                // We read each JSON property by key to verify that the format is correct.
                // Automatically deserializing could possible use different keys. 
                var status = jsonAnalysisResult["status"].ToObject<string>();
                var comments = jsonAnalysisResult["comments"].ToObject<string[]>();

                return new TestSolutionAnalysisRun(returnCode, status, comments);
            }
        }
    }
}