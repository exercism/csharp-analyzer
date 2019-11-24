using System.IO;
using System.Text.Json;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionsAnalysisRunWriter
    {
        public static void Write(BulkSolutionsAnalysisRun analysisRun)
        {
            using (var fileStream = File.Create(Path.Combine(analysisRun.Options.Directory, "bulk_analysis.json")))
            using (var jsonTextWriter = new Utf8JsonWriter(fileStream, new JsonWriterOptions { Indented = true }))
            {
                JsonSerializer.Serialize(jsonTextWriter, analysisRun);
            }
        }
    }
}