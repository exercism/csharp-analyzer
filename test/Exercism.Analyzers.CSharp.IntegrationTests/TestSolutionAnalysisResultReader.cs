using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                // We read the JSON manually to verify that the format is correct
                var jsonAnalysisResult = JToken.ReadFrom(jsonReader);

                return new TestSolutionAnalysisResult(jsonAnalysisResult.ParseStatus(), jsonAnalysisResult.ParseComments());
            }
        }
        
        private static string ParseStatus(this JToken jsonAnalysisResult) =>
            jsonAnalysisResult["status"].ToObject<string>();

        private static TestSolutionComment[] ParseComments(this JToken jsonAnalysisResult) =>
            jsonAnalysisResult["comments"].Select(ParseComment).ToArray();

        private static TestSolutionComment ParseComment(JToken comment) =>
            new TestSolutionComment(comment.ParseCommentText(), comment.ParseCommentParameters());

        private static string ParseCommentText(this JToken comment) =>
            comment["comment"].ToString();

        private static Dictionary<string, string> ParseCommentParameters(this JToken comment) =>
            comment["params"].ToObject<Dictionary<string, string>>();
    }
}