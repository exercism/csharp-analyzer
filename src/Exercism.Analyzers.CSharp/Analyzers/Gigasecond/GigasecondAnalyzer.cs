using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new GigasecondSolution(parsedSolution));

        private static SolutionAnalysis Analyze(GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.DisapproveWhenInvalid() ??
            gigasecondSolution.ApproveWhenValid() ??
            gigasecondSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.DisapproveWhenCreatingNewDateTime() ??
            gigasecondSolution.DisapproveWhenNotUsingAddSeconds();

        private static SolutionAnalysis DisapproveWhenNotUsingAddSeconds(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.DoesNotUseAddSeconds()
                ? gigasecondSolution.DisapproveWithComment(UseAddSeconds)
                : null;

        private static SolutionAnalysis DisapproveWhenCreatingNewDateTime(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.CreatesNewDatetime()
                ? gigasecondSolution.DisapproveWithComment(DontCreateDateTime)
                : null;

        private static SolutionAnalysis ApproveWhenValid(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.ApproveWhenUsingAddSecondsWithScientificNotation() ??
            gigasecondSolution.ApproveWhenUsingDigitsWithoutSeparator() ??
            gigasecondSolution.ApproveWhenUsingAddSecondsWithDigitsWithSeparator() ??
            gigasecondSolution.ApproveWhenUsingAddSecondsWithMathPow();

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithScientificNotation(this GigasecondSolution gigasecondSolution)
        {
            if (!gigasecondSolution.UsesAddSecondsWithScientificNotation())
                return null;

            if (gigasecondSolution.UsesParameterInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(InlineParameter);
            
            if (gigasecondSolution.UsesVariableInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(InlineVariable);

            return gigasecondSolution.UsesExpressionBody()
                ? gigasecondSolution.ApproveAsOptimal()
                : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);
        }

        private static SolutionAnalysis ApproveWhenUsingDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithDigitsWithoutSeparator()
                ? gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators)
                : null;

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution)
        {
            if (!gigasecondSolution.UsesAddSecondsWithDigitsWithSeparator())
                return null;

            if (gigasecondSolution.UsesParameterInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(InlineParameter);

            if (gigasecondSolution.UsesVariableInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(InlineVariable);

            return gigasecondSolution.UsesExpressionBody()
                ? gigasecondSolution.ApproveAsOptimal()
                : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);
        }

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithMathPow() 
                ? gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow)
                : null;
    }
}