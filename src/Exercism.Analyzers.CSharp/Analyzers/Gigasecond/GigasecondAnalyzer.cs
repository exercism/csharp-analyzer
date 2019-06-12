using System.Collections.Generic;
using System.Linq;
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
            gigasecondSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this GigasecondSolution gigasecondSolution)
        {
            var comments = new List<string>();

            if (gigasecondSolution.CreatesNewDatetime())
                comments.Add(DontCreateDateTime);
            
            if (gigasecondSolution.DoesNotUseAddSeconds())
                comments.Add(UseAddSeconds);

            return comments.Any() ? gigasecondSolution.DisapproveWithComment(comments.ToArray()) : null;
        }

        private static SolutionAnalysis ApproveWhenValid(this GigasecondSolution gigasecondSolution)
        {
            var comments = new List<string>();

            if (gigasecondSolution.UsesMathPow())
                comments.Add(UseScientificNotationNotMathPow);

            if (gigasecondSolution.UsesDigitsWithoutSeparator())
                comments.Add(UseScientificNotationOrDigitSeparators);

            if (gigasecondSolution.AssignsToParameterAndReturns() ||
                gigasecondSolution.AssignsToVariableAndReturns())
                comments.Add(ReturnImmediately);
                    
            if (gigasecondSolution.UsesLocalVariable() &&
                !gigasecondSolution.UsesLocalConstVariable())
                comments.Add(UseConstant);
                
            if (gigasecondSolution.UsesField() &&
                !gigasecondSolution.UsesConstField())
                comments.Add(UseConstant);

            if (gigasecondSolution.UsesField() &&
                !gigasecondSolution.UsesPrivateField())
                comments.Add(UsePrivateVisibility);

            if (gigasecondSolution.UsesSingleLine() &&
                !gigasecondSolution.UsesExpressionBody())
                comments.Add(UseExpressionBodiedMember);

            if (comments.Any())
                return gigasecondSolution.ApproveWithComment(comments.ToArray());

            if (gigasecondSolution.UsesScientificNotation() ||
                gigasecondSolution.UsesDigitsWithSeparator())
                return gigasecondSolution.ApproveAsOptimal();

            return null;
        }
    }
}