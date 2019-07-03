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
            Assert.Equal(Serialize(analysisRun.Expected), Serialize(analysisRun.Actual));
        }

        private static string Serialize(TestSolutionAnalysisResult result) =>
            JsonConvert.SerializeObject(result, Formatting.Indented, CreateJsonSerializerSettings());

        private static JsonSerializerSettings CreateJsonSerializerSettings() =>
            new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }};
    }
}