using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Exercism.Analyzers.CSharp
{
    internal class SolutionComment
    {
        public string Comment { get; }
        public IDictionary<string, string> Parameters { get; }

        public SolutionComment(string comment, IDictionary<string, string> parameters = null) =>
            (Comment, Parameters) = (comment, parameters ?? new Dictionary<string, string>());

        public void Add(string key, string value) => Parameters.Add(key, value);
    }
}