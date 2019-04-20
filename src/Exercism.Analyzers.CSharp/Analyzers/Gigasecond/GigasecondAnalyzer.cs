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
            gigasecondSolution.ApproveWhenUsingAddSecondsWithDigitsWithoutSeparator() ??
            gigasecondSolution.ApproveWhenUsingAddSecondsWithDigitsWithSeparator() ??
            gigasecondSolution.ApproveWhenUsingAddSecondsWithMathPow();

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithScientificNotation(this GigasecondSolution gigasecondSolution)
        {
            if (!gigasecondSolution.UsesAddSecondsWithScientificNotation() &&
                !gigasecondSolution.UsesAddSecondsWithScientificNotationVariable())
                return null;

            if (gigasecondSolution.UsesParameterInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(ReturnImmediately);
            
            if (gigasecondSolution.ReturnsVariableInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(UseConstant);
            
            if (gigasecondSolution.UsesVariableInReturnedExpression &&
                gigasecondSolution.VariableDefinedInLocalDeclaration)
                    return gigasecondSolution.VariableIsConstant
                        ? gigasecondSolution.ApproveAsOptimal()
                        : gigasecondSolution.ApproveWithComment(UseConstant);
            
            return gigasecondSolution.UsesExpressionBody()
                ? gigasecondSolution.ApproveAsOptimal()
                : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);
        }

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithDigitsWithoutSeparator() || 
            gigasecondSolution.UsesAddSecondsWithDigitsWithoutSeparatorVariable()
                ? gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators)
                : null;

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithDigitsWithSeparator(this GigasecondSolution gigasecondSolution)
        {
            if (!gigasecondSolution.UsesAddSecondsWithDigitsWithSeparator() &&
                !gigasecondSolution.UsesAddSecondsWithDigitsWithSeparatorVariable())
                return null;

            if (gigasecondSolution.UsesParameterInReturnedExpression ||
                gigasecondSolution.ReturnsVariableInReturnedExpression)
                return gigasecondSolution.ApproveWithComment(ReturnImmediately);

            return gigasecondSolution.UsesExpressionBody()
                ? gigasecondSolution.ApproveAsOptimal()
                : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);
        }

        private static SolutionAnalysis ApproveWhenUsingAddSecondsWithMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesAddSecondsWithMathPow() ||
            gigasecondSolution.UsesAddSecondsWithMathPowVariable() 
                ? gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow)
                : null;
    }
}