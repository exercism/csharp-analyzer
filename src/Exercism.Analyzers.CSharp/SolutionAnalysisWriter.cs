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
                jsonTextWriter.WriteStatus(solutionAnalysis.Result.Status);
                jsonTextWriter.WriteComments(solutionAnalysis.Result.Comments);
                jsonTextWriter.WriteEndObject();
            }
        }

        private static void WriteStatus(this JsonTextWriter jsonTextWriter, SolutionStatus status)
        {
            jsonTextWriter.WritePropertyName("status");
            jsonTextWriter.WriteValue(status.ToString().Underscore());
        }

        private static void WriteComments(this JsonTextWriter jsonTextWriter, SolutionComment[] comments)
        {
            jsonTextWriter.WritePropertyName("comments");
            jsonTextWriter.WriteStartArray();

            foreach (var comment in comments)
                jsonTextWriter.WriteComment(comment);
            
            jsonTextWriter.WriteEndArray();
        }

        private static void WriteComment(this JsonTextWriter jsonTextWriter, SolutionComment comment)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WriteCommentText(comment);
            jsonTextWriter.WriteCommentParameters(comment);
            jsonTextWriter.WriteEndObject();
        }

        private static void WriteCommentText(this JsonTextWriter jsonTextWriter, SolutionComment comment)
        {
            jsonTextWriter.WritePropertyName("comment");
            jsonTextWriter.WriteValue(comment.Comment);
        }

        private static void WriteCommentParameters(this JsonTextWriter jsonTextWriter, SolutionComment comment)
        {
            jsonTextWriter.WritePropertyName("params");
            jsonTextWriter.WriteStartObject();
            
            foreach (var parameter in comment.Parameters)
                jsonTextWriter.WriteCommentParameter(parameter);
            
            jsonTextWriter.WriteEndObject();
        }

        private static void WriteCommentParameter(this JsonTextWriter jsonTextWriter, SolutionCommentParameter parameter)
        {
            jsonTextWriter.WritePropertyName(parameter.Key);
            jsonTextWriter.WriteValue(parameter.Value);
        }
    }
}