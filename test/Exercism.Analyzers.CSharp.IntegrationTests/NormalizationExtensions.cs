using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class NormalizationExtensions
    {
        public static string NormalizeMarkdown(this string markdown) =>
            markdown.Replace(Environment.NewLine, "\n").Trim();

        public static string NormalizeJson(this string json) =>
            JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.None, CreateJsonSerializerSettings());

        private static JsonSerializerSettings CreateJsonSerializerSettings() =>
            new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() } };
    }
}