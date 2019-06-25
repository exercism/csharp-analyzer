using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisResultReader
    {
        public static BulkSolutionAnalysisResult Read(string filePath)
        {
            using (var fileReader = File.OpenText(filePath))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                // We read each JSON property by key to verify that the format is correct.
                // Automatically deserializing could possible use different keys.
                var jsonAnalysisResult = JToken.ReadFrom(jsonReader);

                return new BulkSolutionAnalysisResult(jsonAnalysisResult.ParseStatus(), jsonAnalysisResult.ParseComments());
            }
        }

        private static string ParseStatus(this JToken jsonAnalysisResult) =>
            jsonAnalysisResult["status"].ToObject<string>();

        private static string[] ParseComments(this JToken jsonAnalysisResult) =>
            jsonAnalysisResult["comments"].Children<JObject>().Select(ParseComment).ToArray();

        private static string ParseComment(this JObject child) =>
            child["comment"].ToObject<string>();
    }
}