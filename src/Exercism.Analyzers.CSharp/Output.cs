using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Exercism.Analyzers.CSharp;

internal static class Output
{
    private static readonly JsonWriterOptions JsonWriterOptions = new()
    {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static void WriteToFile(Options options, Analysis analysis)
    {
        using var fileStream = File.Create(GetAnalysisFilePath(options));
        var jsonWriter = new Utf8JsonWriter(fileStream, JsonWriterOptions);
        jsonWriter.WriteStartObject();
        jsonWriter.WriteComments(analysis.Comments);
        jsonWriter.WriteTags(analysis.Tags);
        jsonWriter.WriteEndObject();
        jsonWriter.Flush();
        fileStream.WriteByte((byte)'\n');
    }

    private static string GetAnalysisFilePath(Options options) =>
        Path.GetFullPath(Path.Combine(options.OutputDirectory, "analysis.json"));

    private static void WriteComments(this Utf8JsonWriter jsonTextWriter, List<Comment> comments)
    {
        jsonTextWriter.WritePropertyName("comments");
        jsonTextWriter.WriteStartArray();

        foreach (var comment in comments.DistinctBy(comment => comment.Text))
            jsonTextWriter.WriteComment(comment);

        jsonTextWriter.WriteEndArray();
    }

    private static void WriteComment(this Utf8JsonWriter jsonTextWriter, Comment comment)
    {
        jsonTextWriter.WriteStartObject();
        jsonTextWriter.WriteCommentText(comment);
        jsonTextWriter.WriteCommentType(comment);
        jsonTextWriter.WriteCommentParameters(comment);
        jsonTextWriter.WriteEndObject();
    }

    private static void WriteCommentText(this Utf8JsonWriter jsonTextWriter, Comment comment) =>
        jsonTextWriter.WriteString("comment", comment.Text);

    private static void WriteCommentType(this Utf8JsonWriter jsonTextWriter, Comment comment) =>
        jsonTextWriter.WriteString("type", comment.Type.ToString().ToLower());

    private static void WriteCommentParameters(this Utf8JsonWriter jsonTextWriter, Comment comment)
    {
        jsonTextWriter.WritePropertyName("params");
        jsonTextWriter.WriteStartObject();

        foreach (var parameter in comment.Parameters)
            jsonTextWriter.WriteCommentParameter(parameter);

        jsonTextWriter.WriteEndObject();
    }

    private static void WriteCommentParameter(this Utf8JsonWriter jsonTextWriter, CommentParameter parameter) =>
        jsonTextWriter.WriteString(parameter.Key, parameter.Value);

    private static void WriteTags(this Utf8JsonWriter jsonTextWriter, List<string> tags)
    {
        jsonTextWriter.WriteStartArray("tags");

        foreach (var tag in tags.ToSortedSet())
            jsonTextWriter.WriteStringValue(tag);

        jsonTextWriter.WriteEndArray();
    }
}

internal static class EnumerableExtensions
{
    public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> enumerable) => new(enumerable);
}
