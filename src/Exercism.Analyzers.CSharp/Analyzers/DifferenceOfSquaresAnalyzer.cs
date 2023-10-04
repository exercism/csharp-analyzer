using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class DifferenceOfSquaresAnalyzer : Analyzer
{
    public DifferenceOfSquaresAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (node.DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Select(GetSymbol)
            .Where(invocationSymbol => invocationSymbol is not null)
            .All(invocationSymbol => invocationSymbol.ContainingType.ToDisplayString() == "System.Math"))
            AddTags(Tags.TechniqueMath);

        base.VisitMethodDeclaration(node);
    }

    private static class Tags
    {
        public const string TechniqueMath = "technique:math";
    }
}
