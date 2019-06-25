using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ExerciseAnalyzerTests
    {
        [Theory]
        [TestSolutionsData]
        public void SolutionShouldBeCorrectlyAnalyzed(TestSolution testSolution)
        {
            var analysisRun = TestSolutionAnalyzer.Run(testSolution);

            var expected = SerializeResult(analysisRun.Expected);
            var actual = SerializeResult(analysisRun.Actual);

            Assert.Equal(expected, actual);
        }

        private static string SerializeResult(TestSolutionAnalysisResult result) =>
            JsonConvert.SerializeObject(result, JsonSerializerSettings());

        private static JsonSerializerSettings JsonSerializerSettings() =>
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
    }
}