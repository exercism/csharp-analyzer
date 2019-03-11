using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class SolutionReader
    {
        public static Solution Read(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Log.Error("Solution directory {Directory} does not exist.", directory);
                return null;
            }
            
            var solutionFilePath = Path.Combine(directory, ".solution.json");
            if (!File.Exists(solutionFilePath))
            {
                Log.Error("Solution file {File} does not exist.", solutionFilePath);
                return null;
            }   
            
            return ReadSolution(directory, solutionFilePath);
        }

        private static Solution ReadSolution(string directory, string solutionFilePath)
        {
            Log.Information("Reading solution from directory {Directory}.");

            using (var fileReader = File.OpenText(solutionFilePath))
            using (var jsonReader = new JsonTextReader(fileReader))
            {
                var jsonSolution = JToken.ReadFrom(jsonReader);
                var track = jsonSolution.Value<string>("track");
                var exercise = jsonSolution.Value<string>("exercise");
                
                Log.Information("Found solution for track {Track} and exercise {Exercise}.", track, exercise);

                if (track != Tracks.CSharp)
                {
                    Log.Error("Cannot analyze {Track} solution.", track);
                    return null;
                }

                return new Solution(exercise, track, directory);
            }
        }
    }
}