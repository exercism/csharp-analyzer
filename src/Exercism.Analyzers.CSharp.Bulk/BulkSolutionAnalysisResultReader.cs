using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionAnalysisResultReader
    {
        public static BulkSolutionAnalysisResult Read(string filePath)
        {
            using (var fileReader = File.OpenRead(filePath))
            {
                // We read each JSON property by key to verify that the format is correct.
                // Automatically deserializing could possible use different keys.
                var jsonDocument = JsonDocument.Parse(fileReader);

                return new BulkSolutionAnalysisResult(jsonDocument.ParseStatus(), jsonDocument.ParseComments());
            }
        }

        private static string ParseStatus(this JsonDocument jsonAnalysisResult) =>
            jsonAnalysisResult.RootElement.GetProperty("status").GetString();

        private static string[] ParseComments(this JsonDocument jsonAnalysisResult) =>
            jsonAnalysisResult.RootElement.GetProperty("comments").EnumerateArray().Select(ParseComment).ToArray();

        private static string ParseComment(this JsonElement child) =>
            child.GetProperty("comment").GetString();
    }
}