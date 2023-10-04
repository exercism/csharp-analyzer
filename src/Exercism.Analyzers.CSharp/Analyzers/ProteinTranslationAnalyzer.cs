using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class ProteinTranslationAnalyzer : Analyzer
{    
    public ProteinTranslationAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        switch (GetSymbolName(node))
        {
            case "string.Substring(int, int)":
                AddTags(Tags.UsesStringSubstring);
                break;
        }

        base.VisitInvocationExpression(node);
    }

    private static class Tags
    {
        public const string UsesStringSubstring = "uses:String.Substring";
    }
}