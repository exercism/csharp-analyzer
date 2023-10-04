using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class ReverseStringAnalyzer : Analyzer
{
    public ReverseStringAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (GetConstructedFromSymbolName(node) == "System.Array.Reverse<T>(T[])")
            AddTags(Tags.UsesArrayReverse);

        base.VisitInvocationExpression(node);
    }

    private static class Tags
    {
        public const string UsesArrayReverse = "uses:Array.Reverse";
    }
}