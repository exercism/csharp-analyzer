using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionsAnalysisRunWriter
    {
        public static void Write(BulkSolutionsAnalysisRun analysisRun)
        {
            using (var fileWriter = File.CreateText(Path.Combine(analysisRun.Options.Directory, "bulk_analysis.json")))
            using (var jsonTextWriter = new JsonTextWriter(fileWriter))
            {
                jsonTextWriter.Formatting = Formatting.Indented;

                var analysisRunJObject = CreateAnalysisRunJObject(analysisRun);
                analysisRunJObject.WriteTo(jsonTextWriter);
            }
        }

        private static JObject CreateAnalysisRunJObject(BulkSolutionsAnalysisRun analysisRun) =>
            JObject.FromObject(analysisRun, CreateJsonSerializer());

        private static JsonSerializer CreateJsonSerializer() =>
            new JsonSerializer
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
    }
}