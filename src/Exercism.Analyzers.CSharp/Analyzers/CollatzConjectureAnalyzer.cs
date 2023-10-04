using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class CollatzConjectureAnalyzer : Analyzer
{
    public CollatzConjectureAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (GetConstructedFromSymbolName(node) == "System.Collections.Generic.IEnumerable<TSource>.Count<TSource>()")
            AddTags(Tags.UsesEnumerableCount);

        base.VisitInvocationExpression(node);
    }

    private static class Tags
    {
        public const string UsesEnumerableCount = "uses:Enumerable.Count";
    }
}