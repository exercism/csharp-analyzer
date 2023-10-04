using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class IsogramAnalyzer : Analyzer
{    
    public IsogramAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (GetSymbolInfo(node).Symbol is IMethodSymbol methodSymbol)
        {
            switch (methodSymbol.ConstructedFrom.ToDisplayString())
            {
                case "System.Collections.Generic.IEnumerable<TSource>.Distinct<TSource>()":
                    AddTags(Tags.UsesEnumerableDistinct);
                    break;
                case "System.Collections.Generic.IEnumerable<TSource>.GroupBy<TSource, TKey>(System.Func<TSource, TKey>)":
                    AddTags(Tags.UsesEnumerableGroupBy);
                    break;
            }
        }

        base.VisitInvocationExpression(node);
    }

    private static class Tags
    {
        public const string UsesEnumerableDistinct = "uses:Enumerable.Distinct";
        public const string UsesEnumerableGroupBy = "uses:Enumerable.GroupBy";
    }
}