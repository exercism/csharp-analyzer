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

        private static SolutionAnalysis DisapproveWhenInvalid(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.CreatesNewDatetime())
                return gigasecondSolution.DisapproveWithComment(DontCreateDateTime);
            
            if (gigasecondSolution.DoesNotUseAddSeconds())
                return gigasecondSolution.DisapproveWithComment(UseAddSeconds);

            return null;
        }

        private static SolutionAnalysis ApproveWhenValid(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.UsesAddSecondsWithMathPow())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.UsesAddSecondsWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            if (!gigasecondSolution.UsesAddSecondsWithScientificNotation() &&
                !gigasecondSolution.UsesAddSecondsWithDigitsWithSeparator())
                return null;

            if (gigasecondSolution.AssignsToParameterAndReturns() ||
                gigasecondSolution.AssignsToVariableAndReturns())
                return gigasecondSolution.ApproveWithComment(ReturnImmediately);
            
            if (gigasecondSolution.UsesAddSecondsWithLocalVariable())
                return gigasecondSolution.UsesConstVariable()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(UseConstant);
            
            if (gigasecondSolution.UsesAddSecondsWithFieldVariable())
                return gigasecondSolution.UsesConstVariable()
                    ? gigasecondSolution.UsesPrivateField()
                        ?  gigasecondSolution.UsesExpressionBody()
                            ? gigasecondSolution.ApproveAsOptimal()
                            : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember)
                        : gigasecondSolution.ApproveWithComment(UsePrivateVisibility)
                    : gigasecondSolution.ApproveWithComment(UseConstant);
            
            return gigasecondSolution.UsesExpressionBody()
                ? gigasecondSolution.ApproveAsOptimal()
                : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);
        }
    }
}