using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class GrainsAnalyzer : Analyzer
{
    public GrainsAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var symbol = SemanticModel.GetSymbolInfo(node.Expression).Symbol;
        if (symbol?.ToDisplayString() == "System.Math.Pow(double, double)")
            AddTags(Tags.UsesMathPow);

        base.VisitInvocationExpression(node);
    }

    public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        var symbol = SemanticModel.GetSymbolInfo(node).Symbol;
        if (symbol?.ToDisplayString() == "ulong.MaxValue")
            AddTags(Tags.UsesUlongMaxValue);
        
        base.VisitMemberAccessExpression(node);
    }

    private static class Tags
    {
        public const string UsesMathPow = "uses:Math.Pow";
        public const string UsesUlongMaxValue = "uses:ulong.MaxValue";
    }
}