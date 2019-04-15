using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new GigasecondSolution(parsedSolution));

        private static SolutionAnalysis Analyze(GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AnalyzeError() ??
            gigasecondSolution.AnalyzeSingleLine() ??
            gigasecondSolution.AnalyzeVariableAssignment() ??
            gigasecondSolution.AnalyzeParameterAssignment() ??
            gigasecondSolution.ReferToMentor();

        private static SolutionAnalysis AnalyzeError(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.UsesAddMethod() ||
                gigasecondSolution.UsesPlusOperator())
                return gigasecondSolution.DisapproveWithComment(UseAddSeconds);
            
            if (gigasecondSolution.CreatesDatetime())
                return gigasecondSolution.DisapproveWithComment(DontCreateDateTime);

            return null;
        }

        private static SolutionAnalysis AnalyzeSingleLine(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.ReturnsAddSecondsWithScientificNotation())
                return gigasecondSolution.UsesExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (gigasecondSolution.ReturnsAddSecondsWithMathPow())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.ReturnsAddSecondsWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            if (gigasecondSolution.ReturnsAddSecondsWithDigitsWithSeparator())
                return gigasecondSolution.UsesExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);

            return null;
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this GigasecondSolution gigasecondSolution)
        {
            if (!gigasecondSolution.AssignsAndReturnsParameter())
                return null;

            if (gigasecondSolution.AssignsParameterUsingAddSecondsWithScientificNotation() ||
                gigasecondSolution.AssignsParameterUsingAddSecondsWithDigitsWithSeparator())
                return gigasecondSolution.ApproveWithComment(InlineParameter);

            if (gigasecondSolution.AssignsParameterUsingAddSecondsWithMathPow())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.AssignsParameterUsingAddSecondsWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            return null;
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this GigasecondSolution gigasecondSolution)
        {
            if (!gigasecondSolution.AssignsAndReturnsVariable())
                return null;

            if (gigasecondSolution.AssignsVariableUsingAddSecondsWithScientificNotation() ||
                gigasecondSolution.AssignsVariableUsingAddSecondsWithDigitsWithSeparator())
                return gigasecondSolution.ApproveWithComment(InlineVariable);

            if (gigasecondSolution.AssignsVariableUsingAddSecondsWithMathPow())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.AssignsVariableUsingAddSecondsWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            return null;
        }
    }
}