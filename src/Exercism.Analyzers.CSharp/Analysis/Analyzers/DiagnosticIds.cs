namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class DiagnosticIds
    {
        // Optimization analyzer IDs
        public const string UseMinimumNumberOfChecksForLeapYearAnalyzerRuleId = "EXC0101";
        
        // Language feature analyzer IDs
        public const string UseExponentNotationAnalyzerRuleId = "EXC0201";
        public const string UseDigitSeparatorAnalyzerRuleId = "EXC0202";
        public const string UseExpressionBodiedMemberAnalyzerRuleId = "EXC0203";

        // API usage analyzer IDs 
        public const string UseAddSecondsToAddGigasecondAnalyzerRuleId = "EXC0301";
    }
}