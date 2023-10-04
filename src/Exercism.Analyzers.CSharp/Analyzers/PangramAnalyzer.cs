using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class PangramAnalyzer : Analyzer
{    
    public PangramAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        switch (GetSymbolName(node))
        {
            case "string.ToLower()":
                AddTags(Tags.UsesStringToLower);
                break;
            case "string.Contains(char)":
                AddTags(Tags.UsesStringContains);
                break;
            case "string.Contains(char, System.StringComparison)":
                AddTags(Tags.UsesStringContainsStringComparison);
                break;
        }
        
        if (GetConstructedFromSymbolName(node) == "System.Collections.Generic.IEnumerable<TSource>.All<TSource>(System.Func<TSource, bool>)")
            AddTags(Tags.UsesEnumerableAll);

        base.VisitInvocationExpression(node);
    }

    private static class Tags
    {
        public const string UsesStringToLower = "uses:String.ToLower";
        public const string UsesStringContains = "uses:String.Contains(char)";
        public const string UsesStringContainsStringComparison = "uses:string.Contains(char, System.StringComparison)";
        public const string UsesEnumerableAll = "uses:Enumerable.All";
    }
}