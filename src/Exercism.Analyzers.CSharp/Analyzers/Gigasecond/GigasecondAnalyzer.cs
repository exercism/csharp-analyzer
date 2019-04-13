using Exercism.Analyzers.CSharp.Analyzers.Shared;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new GigasecondSolution(parsedSolution));

        private static SolutionAnalysis Analyze(GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.ReturnsAddSecondsWithScientificNotation())
                return gigasecondSolution.UsesExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);

            if (gigasecondSolution.ReturnsAddSecondsWithMathPow())
                return gigasecondSolution.ApproveWithComment(GigasecondComments.UseScientificNotationNotMathPow);

            if (gigasecondSolution.ReturnsAddSecondsWithDigitsWithoutSeparator())
                return gigasecondSolution.ApproveWithComment(GigasecondComments.UseScientificNotationOrDigitSeparators);

            if (gigasecondSolution.ReturnsAddSecondsWithDigitsWithSeparator())
                return gigasecondSolution.UsesExpressionBody()
                    ? gigasecondSolution.ApproveAsOptimal()
                    : gigasecondSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);
            
            if (gigasecondSolution.UsesAddMethod() ||
                gigasecondSolution.UsesPlusOperator())
                return gigasecondSolution.DisapproveWithComment(GigasecondComments.UseAddSeconds);

            return gigasecondSolution.ReferToMentor();
        }
    }
}