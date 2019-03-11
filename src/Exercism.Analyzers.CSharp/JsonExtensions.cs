using System.Collections.Generic;
using Newtonsoft.Json;

namespace Exercism.Analyzers.CSharp
{
    internal static class JsonExtensions
    {
        public static void WriteValues<T>(this JsonTextWriter jsonTextWriter, IEnumerable<T> values)
        {
            foreach (var value in values)
                jsonTextWriter.WriteValue(value);
        }
    }
}