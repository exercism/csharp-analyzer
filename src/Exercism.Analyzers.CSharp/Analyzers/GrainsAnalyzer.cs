using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class GrainsAnalyzer : Analyzer
{
    public GrainsAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (GetSymbolName(node.Expression) == "System.Math.Pow(double, double)")
            AddTags(Tags.UsesMathPow);

        base.VisitInvocationExpression(node);
    }

    public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
    {
        if (GetSymbolName(node) == "ulong.MaxValue")
            AddTags(Tags.UsesUlongMaxValue);
        
        base.VisitMemberAccessExpression(node);
    }

    private static class Tags
    {
        public const string UsesMathPow = "uses:Math.Pow";
        public const string UsesUlongMaxValue = "uses:ulong.MaxValue";
    }
}