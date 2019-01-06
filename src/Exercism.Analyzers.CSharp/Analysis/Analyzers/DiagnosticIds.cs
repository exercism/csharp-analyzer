namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    internal static class DiagnosticIds
    {
        // Health analyzer IDs
        public const string CompilesSuccessfullyAnalyzerRuleId = "EXC0001";
        public const string AllTestPassAnalyzerRuleId = "EXC0002";
        
        // Optimization analyzer IDs
        public const string UseMinimumNumberOfChecksForLeapYearAnalyzerRuleId = "EXC0101";
        
        // Language feature analyzer IDs
        public const string UseExponentNotationAnalyzerRuleId = "EXC0201";
        public const string UseDigitSeparatorAnalyzerRuleId = "EXC0202";
        public const string UseExpressionBodiedMemberAnalyzerRuleId = "EXC0203";
    }
}