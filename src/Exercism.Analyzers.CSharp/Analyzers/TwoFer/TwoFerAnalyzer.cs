using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(TwoFerSolution solution) =>
            solution.DisapproveWhenInvalid() ??
            solution.ApproveWhenValid() ??
            solution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this TwoFerSolution solution)
        {
            if (solution.UsesOverloads)
                solution.AddComment(UseDefaultValueNotOverloads);

            if (solution.MissingSpeakMethod ||
                solution.InvalidSpeakMethod)
                solution.AddComment(FixCompileErrors);

            if (solution.UsesDuplicateString)
                solution.AddComment(UseSingleFormattedStringNotMultiple);

            if (solution.NoDefaultValue)
                solution.AddComment(UseDefaultValue(solution.SpeakMethodParameterName));

            if (solution.UsesInvalidDefaultValue)
                solution.AddComment(InvalidDefaultValue(solution.SpeakMethodParameterName, solution.SpeakMethodParameterDefaultValue));

            if (solution.UsesStringReplace)
                solution.AddComment(UseStringInterpolationNotStringReplace);

            if (solution.UsesStringJoin)
                solution.AddComment(UseStringInterpolationNotStringJoin);

            if (solution.UsesStringConcat)
                solution.AddComment(UseStringInterpolationNotStringConcat);

            return solution.HasComments
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this TwoFerSolution solution) =>
            solution.AnalyzeSingleLine() ??
            solution.AnalyzeParameterAssignment() ??
            solution.AnalyzeVariableAssignment();

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution solution)
        {
            if (!solution.UsesSingleLine)
                return null;

            if (solution.ReturnsStringInterpolationWithIsNullOrEmptyCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (solution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (solution.ReturnsStringInterpolationWithNullCheck)
                solution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (solution.ReturnsStringConcatenation)
                solution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (solution.ReturnsStringFormat)
                solution.AddComment(UseStringInterpolationNotStringFormat);

            if (!solution.UsesExpressionBody)
                solution.AddComment(UseExpressionBodiedMember(solution.SpeakMethodName));

            if (solution.ReturnsStringInterpolationWithDefaultValue ||
                solution.ReturnsStringInterpolationWithNullCoalescingOperator ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution solution)
        {
            if (!solution.AssignsToParameter)
                return null;

            if (!solution.AssignsParameterUsingKnownExpression)
                return solution.ReferToMentor();

            if (solution.ReturnsStringFormat)
                solution.AddComment(UseStringInterpolationNotStringFormat);

            if (solution.ReturnsStringConcatenation)
                solution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (solution.AssignsParameterUsingNullCoalescingOperator)
                solution.AddComment(DoNotAssignAndReturn);

            if (solution.AssignsParameterUsingNullCheck ||
                solution.AssignsParameterUsingIfNullCheck)
                solution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (solution.AssignsParameterUsingIsNullOrEmptyCheck ||
                solution.AssignsParameterUsingIfIsNullOrEmptyCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (solution.AssignsParameterUsingIsNullOrWhiteSpaceCheck ||
                solution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (solution.ReturnsStringInterpolation ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution solution)
        {
            if (!solution.AssignsVariable)
                return null;

            if (!solution.AssignsVariableUsingKnownInitializer)
                return solution.ReferToMentor();

            if (solution.ReturnsStringFormatWithVariable)
                solution.AddComment(UseStringInterpolationNotStringFormat);

            if (solution.ReturnsStringConcatenationWithVariable)
                solution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (solution.AssignsVariableUsingNullCheck)
                solution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (solution.AssignsVariableUsingIsNullOrEmptyCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (solution.AssignsVariableUsingIsNullOrWhiteSpaceCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (solution.ReturnsStringInterpolationWithVariable &&
                solution.AssignsVariableUsingNullCoalescingOperator ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }
    }
}