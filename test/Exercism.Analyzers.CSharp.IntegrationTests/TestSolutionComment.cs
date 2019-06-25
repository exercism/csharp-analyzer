using System.Collections.Generic;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionComment
    {
        public string Comment { get; }
        public IDictionary<string, string> Parameters { get; }

        public TestSolutionComment(string comment, IDictionary<string, string> parameters) =>
            (Comment, Parameters) = (comment, parameters);
    }
}