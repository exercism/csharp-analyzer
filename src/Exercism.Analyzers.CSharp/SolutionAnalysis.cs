using System.IO;
using System.Text.Json;

using Exercism.Analyzers.CSharp.Analyzers.Default;
using Exercism.Analyzers.CSharp.Analyzers.Gigasecond;
using Exercism.Analyzers.CSharp.Analyzers.Leap;
using Exercism.Analyzers.CSharp.Analyzers.TwoFer;
using Exercism.Analyzers.CSharp.Analyzers.WeighingMachine;

using Humanizer;

namespace Exercism.Analyzers.CSharp;

internal record SolutionAnalysis(SolutionStatus Status, SolutionComment[] Comments);

internal static class SolutionAnalyzer
{
    public static SolutionAnalysis Analyze(Solution solution)
    {
        switch (solution.Slug)
        {
            case Exercises.TwoFer:
                return new TwoFerAnalyzer().Analyze(new TwoFerSolution(solution));
            case Exercises.Gigasecond:
                return new GigasecondAnalyzer().Analyze(new GigasecondSolution(solution));
            case Exercises.Leap:
                return new LeapAnalyzer().Analyze(new LeapSolution(solution));
            case Exercises.WeighingMachine:
                return new WeighingMachineAnalyzer().Analyze(new WeighingMachineSolution(solution));
            default:
                return new DefaultExerciseAnalyzer().Analyze(new DefaultSolution(solution));
        }
    }
}

internal static class SolutionAnalysisWriter
{
    private static readonly JsonWriterOptions JsonWriterOptions = new() {Indented = true};
    
    public static void WriteToFile(Options options, SolutionAnalysis solutionAnalysis)
    {
        using var fileStream = File.Create(GetAnalysisFilePath(options));
        using var jsonWriter = new Utf8JsonWriter(fileStream, JsonWriterOptions);
        jsonWriter.WriteStartObject();
        jsonWriter.WriteStatus(solutionAnalysis.Status);
        jsonWriter.WriteComments(solutionAnalysis.Comments);
        jsonWriter.WriteEndObject();
        jsonWriter.Flush();
        fileStream.WriteByte((byte)'\n');
    }

    private static string GetAnalysisFilePath(Options options) =>
        Path.GetFullPath(Path.Combine(options.OutputDirectory, "analysis.json"));

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
