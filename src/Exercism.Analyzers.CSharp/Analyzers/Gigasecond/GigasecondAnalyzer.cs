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
            if (gigasecondSolution.UsesMathPow())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.UsesDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(UseScientificNotationOrDigitSeparators);

            if (!gigasecondSolution.UsesScientificNotation() &&
                !gigasecondSolution.UsesDigitsWithSeparator())
                return null;

            if (gigasecondSolution.AssignsToParameterAndReturns() ||
                gigasecondSolution.AssignsToVariableAndReturns())
                return gigasecondSolution.ApproveWithComment(ReturnImmediately);
            
            if (gigasecondSolution.UsesLocalConstVariable())
                return gigasecondSolution.ApproveAsOptimal();
                    
            if (gigasecondSolution.UsesLocalVariable())
                return gigasecondSolution.ApproveWithComment(UseConstant);

            if (gigasecondSolution.UsesPrivateConstField())
                return gigasecondSolution.UsesExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (gigasecondSolution.UsesConstField())
                return gigasecondSolution.ApproveWithComment(UsePrivateVisibility);
            
            if (gigasecondSolution.UsesField())
                return gigasecondSolution.ApproveWithComment(UseConstant);
            
            return gigasecondSolution.UsesExpressionBody()
                ? gigasecondSolution.ApproveAsOptimal()
                : gigasecondSolution.ApproveWithComment(UseExpressionBodiedMember);
        }
    }
}