using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public static class TestSolutionAnalyzer
    {
        public static TestSolutionAnalysisRun Run(string exercise, string code) =>
            Run(new TestSolution(exercise), code);

        public static TestSolutionAnalysisRun Run(TestSolution testSolution, string code)
        {
            testSolution.CreateFiles(code);

            var returnCode = Program.Main(new[] {testSolution.Paths.Directory});
            if (returnCode > 0)
                return FailedRun(returnCode);

            return SuccessfullRun(returnCode, testSolution);
        }

        private static TestSolutionAnalysisRun FailedRun(int returnCode) =>
            new TestSolutionAnalysisRun(returnCode, approved: false, referToMentor: false, messages: Array.Empty<string>());

        private static TestSolutionAnalysisRun SuccessfullRun(int returnCode, Solution solution)
        {
            using (var fileReader = File.OpenText(solution.Paths.AnalysisFilePath))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var jsonAnalysisResult = JToken.ReadFrom(jsonReader);

                // We read each JSON property by key to verify that the format is correct.
                // Automatically deserializing could possible use different keys. 
                var approve = jsonAnalysisResult["approve"].ToObject<bool>();
                var referToMentor = jsonAnalysisResult["refer_to_mentor"].ToObject<bool>();
                var messages = jsonAnalysisResult["messages"].ToObject<string[]>();

                return new TestSolutionAnalysisRun(returnCode, approve, referToMentor, messages);
            }
        }
    }
}