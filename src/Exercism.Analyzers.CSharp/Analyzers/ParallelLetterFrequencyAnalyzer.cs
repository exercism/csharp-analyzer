using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class ParallelLetterFrequencyAnalyzer : Analyzer
{
    public ParallelLetterFrequencyAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (GetConstructedFromSymbolName(node) == "System.Collections.Generic.IEnumerable<TSource>.AsParallel<TSource>()")
            AddTags(Tags.UsesEnumerableAsParallel);

        base.VisitInvocationExpression(node);
    }

    private static class Tags
    {
        public const string UsesEnumerableAsParallel = "uses:Enumerable.AsParallel";
    }
}