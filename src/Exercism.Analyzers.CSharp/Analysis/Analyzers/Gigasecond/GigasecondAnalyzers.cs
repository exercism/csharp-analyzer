using Microsoft.CodeAnalysis.Diagnostics;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzers
    {
        public static readonly DiagnosticAnalyzer[] All = 
        {
            new UseAddSecondsToAddGigasecondAnalyzer(), 
        };
        
        public static readonly GigasecondApprovalAnalyzer ApprovalAnalyzer = new GigasecondApprovalAnalyzer();
    }
}