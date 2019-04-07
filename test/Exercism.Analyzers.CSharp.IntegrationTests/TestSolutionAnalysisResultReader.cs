using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionAnalysisResultReader
    {
        public static TestSolutionAnalysisResult Read(string filePath)
        {
            using (var fileReader = File.OpenText(filePath))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var jsonAnalysisResult = JToken.ReadFrom(jsonReader);

                // We read each JSON property by key to verify that the format is correct.
                // Automatically deserializing could possible use different keys. 
                var status = jsonAnalysisResult["status"].ToObject<string>();
                var comments = jsonAnalysisResult["comments"].ToObject<string[]>();

                return new TestSolutionAnalysisResult(status, comments);
            }
        }
    }
}