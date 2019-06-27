using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(GigasecondSolutionParser.Parse(parsedSolution));

        private static SolutionAnalysis Analyze(GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.DisapproveWhenInvalid() ??
            gigasecondSolution.ApproveWhenValid() ??
            gigasecondSolution.ApproveWhenOptimal() ??
            gigasecondSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.CreatesNewDatetime())
                gigasecondSolution.AddComment(DoNotCreateDateTime);
            
            if (gigasecondSolution.DoesNotUseAddSeconds())
                gigasecondSolution.AddComment(UseAddSeconds);

            return gigasecondSolution.HasComments()
                ? gigasecondSolution.DisapproveWithComment()
                : gigasecondSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.UsesMathPow())
                gigasecondSolution.AddComment(UseScientificNotationNotMathPow);

            if (gigasecondSolution.UsesDigitsWithoutSeparator())
                gigasecondSolution.AddComment(UseScientificNotationOrDigitSeparators);

            if (gigasecondSolution.AssignsToParameterAndReturns() ||
                gigasecondSolution.AssignsToVariableAndReturns())
                gigasecondSolution.AddComment(ReturnImmediately);
                    
            if (gigasecondSolution.UsesLocalVariable() &&
                !gigasecondSolution.UsesLocalConstVariable())
                gigasecondSolution.AddComment(UseConstant);
                
            if (gigasecondSolution.UsesField() &&
                !gigasecondSolution.UsesConstField())
                gigasecondSolution.AddComment(UseConstant);

            if (gigasecondSolution.UsesField() &&
                !gigasecondSolution.UsesPrivateField())
                gigasecondSolution.AddComment(UsePrivateVisibility);

            if (gigasecondSolution.UsesSingleLine() &&
                !gigasecondSolution.UsesExpressionBody())
                gigasecondSolution.AddComment(UseExpressionBodiedMember);

            return gigasecondSolution.HasComments() ?
                gigasecondSolution.ApproveWithComment() :
                gigasecondSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenOptimal(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.UsesScientificNotation() ||
                gigasecondSolution.UsesDigitsWithSeparator())
                return gigasecondSolution.ApproveAsOptimal();

            return gigasecondSolution.ContinueAnalysis();
        }
    }
}