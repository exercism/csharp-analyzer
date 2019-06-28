namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal class TestSolutionCommentParameter
    {
        public string Key { get; }
        public string Value { get; }

        public TestSolutionCommentParameter(string key, string value) =>
            (Key, Value) = (key, value);
    }
}