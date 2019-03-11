using System.IO;
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
                jsonTextWriter.WritePropertyName("approve");
                jsonTextWriter.WriteValue(analyzedSolution.Status == SolutionStatus.Approve);
                jsonTextWriter.WritePropertyName("refer_to_mentor");
                jsonTextWriter.WriteValue(analyzedSolution.Status == SolutionStatus.ReferToMentor);
                jsonTextWriter.WritePropertyName("messages");
                jsonTextWriter.WriteStartArray();
                jsonTextWriter.WriteValues(analyzedSolution.Messages);
                jsonTextWriter.WriteEndArray();
                jsonTextWriter.WriteEndObject();
            }
        }
    }
}