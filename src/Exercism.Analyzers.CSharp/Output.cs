using System.IO;
using System.Text.Json;

namespace Exercism.Analyzers.CSharp;

internal static class Output
{
    private static readonly JsonWriterOptions JsonWriterOptions = new() {Indented = true};
    
    public static void WriteToFile(Options options, SolutionAnalysis solutionAnalysis)
    {
        using var fileStream = File.Create(GetAnalysisFilePath(options));
        using var jsonWriter = new Utf8JsonWriter(fileStream, JsonWriterOptions);
        jsonWriter.WriteStartObject();
        jsonWriter.WriteComments(solutionAnalysis.Comments);
        jsonWriter.WriteTags(solutionAnalysis.Tags);
        jsonWriter.WriteEndObject();
        jsonWriter.Flush();
        fileStream.WriteByte((byte)'\n');
    }

    private static string GetAnalysisFilePath(Options options) =>
        Path.GetFullPath(Path.Combine(options.OutputDirectory, "analysis.json"));

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
        jsonTextWriter.WriteCommentType(comment);
        jsonTextWriter.WriteCommentParameters(comment);
        jsonTextWriter.WriteEndObject();
    }

    private static void WriteCommentText(this Utf8JsonWriter jsonTextWriter, SolutionComment comment) =>
        jsonTextWriter.WriteString("comment", comment.Text);

    private static void WriteCommentType(this Utf8JsonWriter jsonTextWriter, SolutionComment comment) =>
        jsonTextWriter.WriteString("type", comment.Type.ToString().ToLower());

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

    private static void WriteTags(this Utf8JsonWriter jsonTextWriter, string[] tags)
    {
        jsonTextWriter.WriteStartArray("tags");

        foreach (var tag in tags)
            jsonTextWriter.WriteStringValue(tag);
        
        jsonTextWriter.WriteEndArray();
    }
}