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

        private static SolutionAnalysis ApproveWhenValid(this TwoFerSolution solution)
        {
            if (solution.UsesStringConcatenation)
                solution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (solution.UsesStringFormat)
                solution.AddComment(UseStringInterpolationNotStringFormat);

            if (solution.UsesIsNullOrEmptyCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (solution.UsesIsNullOrWhiteSpaceCheck)
                solution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (solution.UsesNullCheck)
                solution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (solution.UsesSingleLine)
                return solution.AnalyzeSingleLine();

            if (solution.AssignsToParameter)
                return solution.AnalyzeParameterAssignment();

            if (solution.AssignsVariable)
                return solution.AnalyzeVariableAssignment();

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution solution)
        {
            if (!solution.UsesExpressionBody)
                solution.AddComment(UseExpressionBodiedMember(solution.SpeakMethodName));

            if (solution.UsesStringInterpolationWithDefaultValue ||
                solution.UsesStringInterpolationWithNullCoalescingOperator ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution solution)
        {
            if (!solution.AssignsParameterUsingKnownExpression)
                return solution.ReferToMentor();

            if (solution.AssignsParameterUsingNullCoalescingOperator)
                solution.AddComment(DoNotAssignAndReturn);

            if (solution.ReturnsStringInterpolation ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution solution)
        {
            if (!solution.AssignsVariableUsingKnownInitializer)
                return solution.ReferToMentor();

            if (solution.ReturnsStringInterpolationWithVariable &&
                solution.AssignsVariableUsingNullCoalescingOperator ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }
    }
}