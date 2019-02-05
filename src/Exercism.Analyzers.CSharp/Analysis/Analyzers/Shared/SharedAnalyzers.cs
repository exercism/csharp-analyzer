using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Shared
{
    internal static class SharedAnalyzers
    {
        public static readonly DiagnosticAnalyzer[] All = 
        {
            new UseExpressionBodiedMemberAnalyzer(),
            new UseDigitSeparatorAnalyzer(),
            new UseExponentNotationAnalyzer(), 
        };
        
        public static readonly DoNotApproveApprovalAnalyzer ApprovalAnalyzer = new DoNotApproveApprovalAnalyzer();
    }
}