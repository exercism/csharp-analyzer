namespace Exercism.Analyzers.CSharp
{
    internal class SolutionCommentParameter
    {
        public string Key { get; }
        public string Value { get; }

        public SolutionCommentParameter(string key, string value) =>
            (Key, Value) = (key, value);
    }
}