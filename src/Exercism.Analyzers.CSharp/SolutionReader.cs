using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp
{
    public static class SolutionReader
    {
        public static Solution Read(string directory)
        {
            using (var fileReader = File.OpenText(Path.Combine(directory, ".solution.json")))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var jsonSolution = JToken.ReadFrom(jsonReader);
                var track = jsonSolution.Value<string>("track");
                var exercise = jsonSolution.Value<string>("exercise");

                return new Solution(exercise, track, directory);
            }
        }
    }
}