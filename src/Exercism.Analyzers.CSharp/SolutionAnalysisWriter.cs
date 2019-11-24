using System.IO;
using System.Text.Json;
using Humanizer;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionAnalysisWriter
    {
        public static void WriteToFile(Options options, SolutionAnalysis solutionAnalysis)
        {
            using (var fileStream = File.Create(GetAnalysisFilePath(options)))
            using (var jsonTextWriter = new Utf8JsonWriter(fileStream))
            {
                jsonTextWriter.WriteStartObject();
                jsonTextWriter.WriteStatus(solutionAnalysis.Status);
                jsonTextWriter.WriteComments(solutionAnalysis.Comments);
                jsonTextWriter.WriteEndObject();
            }
        }

        private static string GetAnalysisFilePath(Options options) =>
            Path.GetFullPath(Path.Combine(options.Directory, "analysis.json"));

        private static void WriteStatus(this Utf8JsonWriter jsonTextWriter, SolutionStatus status) =>
            jsonTextWriter.WriteString("status", status.ToString().Underscore());

        private static void WriteComments(this Utf8JsonWriter jsonTextWriter, SolutionComment[] comments)
        {
            jsonTextWriter.WritePropertyName("comments");
            jsonTextWriter.WriteStartArray();

            foreach (var comment in comments)
                jsonTextWriter.WriteComment(comment);

            jsonTextWriter.WriteEndArray();
        }

        private static void WriteComment(this Utf8JsonWriter jsonTextWriter, SolutionComment comment)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WriteCommentText(comment);
            jsonTextWriter.WriteCommentParameters(comment);
            jsonTextWriter.WriteEndObject();
        }

        private static void WriteCommentText(this Utf8JsonWriter jsonTextWriter, SolutionComment comment) =>
            jsonTextWriter.WriteString("comment", comment.Comment);

        private static void WriteCommentParameters(this Utf8JsonWriter jsonTextWriter, SolutionComment comment)
        {
            jsonTextWriter.WritePropertyName("params");
            jsonTextWriter.WriteStartObject();

            foreach (var parameter in comment.Parameters)
                jsonTextWriter.WriteCommentParameter(parameter);

            jsonTextWriter.WriteEndObject();
        }

        private static void WriteCommentParameter(this Utf8JsonWriter jsonTextWriter, SolutionCommentParameter parameter) =>
            jsonTextWriter.WriteString(parameter.Key, parameter.Value);
    }
}