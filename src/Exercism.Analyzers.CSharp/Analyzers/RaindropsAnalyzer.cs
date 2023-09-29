using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class RaindropsAnalyzer : Analyzer
{
    public RaindropsAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
        if (symbol is IMethodSymbol methodSymbol && 
            methodSymbol.ConstructedFrom.ReceiverType?.ToDisplayString() == "System.Collections.Generic.IEnumerable<TSource>" &&
            methodSymbol.ConstructedFrom.Name == "Aggregate")
            AddTags(Tags.UsesEnumerableAggregate);

        base.VisitInvocationExpression(node);
    }
    
    private static class Tags
    {
        public const string UsesEnumerableAggregate = "uses:Enumerable.Aggregate";
    }
}