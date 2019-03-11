using System.IO;
using Humanizer;
using Newtonsoft.Json;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class AnalyzedSolutionWriter
    {
        public static void Write(AnalyzedSolution analyzedSolution)
        {
            Log.Information("Writing analyzed solution to analysis file.");
            
            using (var fileWriter = File.CreateText(analyzedSolution.Solution.Paths.AnalysisFilePath))
            using (var jsonTextWriter = new JsonTextWriter(fileWriter))
            {
                jsonTextWriter.WriteStartObject();
                jsonTextWriter.WritePropertyName("status");
                jsonTextWriter.WriteValue(analyzedSolution.Status.ToString().Underscore());
                jsonTextWriter.WritePropertyName("comments");
                jsonTextWriter.WriteStartArray();
                jsonTextWriter.WriteValues(analyzedSolution.Comments);
                jsonTextWriter.WriteEndArray();
                jsonTextWriter.WriteEndObject();
            }
        }
    }
}