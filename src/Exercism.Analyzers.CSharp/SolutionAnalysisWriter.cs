using System.IO;
using Humanizer;
using Newtonsoft.Json;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalysisWriter
    {
        public static void Write(SolutionAnalysis solutionAnalysis)
        {
            Log.Information("Writing analyzed solution to analysis file.");
            
            using (var fileWriter = File.CreateText(solutionAnalysis.Solution.Paths.AnalysisFilePath))
            using (var jsonTextWriter = new JsonTextWriter(fileWriter))
            {
                jsonTextWriter.WriteStartObject();
                jsonTextWriter.WritePropertyName("status");
                jsonTextWriter.WriteValue(solutionAnalysis.Result.Status.ToString().Underscore());
                jsonTextWriter.WritePropertyName("comments");
                jsonTextWriter.WriteStartArray();
                jsonTextWriter.WriteValues(solutionAnalysis.Result.Comments);
                jsonTextWriter.WriteEndArray();
                jsonTextWriter.WriteEndObject();
            }
        }
    }
}