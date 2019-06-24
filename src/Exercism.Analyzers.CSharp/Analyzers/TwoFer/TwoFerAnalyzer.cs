using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(TwoFerSolutionParser.Parse(parsedSolution));

        private static SolutionAnalysis Analyze(TwoFerSolution twoFerSolution) =>
            twoFerSolution.DisapproveWhenInvalid() ??
            twoFerSolution.ApproveWhenValid() ??
            twoFerSolution.ApproveWhenOptimal() ??
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.UsesOverloads)
                twoFerSolution.AddComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingSpeakMethod ||
                twoFerSolution.InvalidSpeakMethod)
                twoFerSolution.AddComment(FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString)
                twoFerSolution.AddComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue)
                twoFerSolution.AddComment(UseDefaultValue);

            if (twoFerSolution.InvalidDefaultValue)
                twoFerSolution.AddComment(InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace)
                twoFerSolution.AddComment(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin)
                twoFerSolution.AddComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat)
                twoFerSolution.AddComment(UseStringInterpolationNotStringConcat);

            return twoFerSolution.HasComments()
                ? twoFerSolution.DisapproveWithComment()
                : twoFerSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenValid(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeSingleLine() ??
            twoFerSolution.AnalyzeParameterAssignment() ??
            twoFerSolution.AnalyzeVariableAssignment();

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.UsesSingleLine())
                return null;

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrEmptyCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithNullCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.ReturnsStringConcatenation())
                twoFerSolution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.ReturnsStringFormat())
                twoFerSolution.AddComment(UseStringInterpolationNotStringFormat);

            if (!twoFerSolution.UsesExpressionBody())
                twoFerSolution.AddComment(UseExpressionBodiedMember);

            return twoFerSolution.HasComments()
                ? twoFerSolution.ApproveWithComment()
                : twoFerSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            if (!twoFerSolution.AssignsParameterUsingKnownExpression())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormat())
                twoFerSolution.AddComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenation())
                twoFerSolution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.AssignsParameterUsingNullCoalescingOperator())
                twoFerSolution.AddComment(ReturnImmediately);

            if (twoFerSolution.AssignsParameterUsingNullCheck() ||
                twoFerSolution.AssignsParameterUsingIfNullCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrEmptyCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            return twoFerSolution.HasComments()
                ? twoFerSolution.ApproveWithComment()
                : twoFerSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsVariable())
                return null;

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormatWithVariable())
                twoFerSolution.AddComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenationWithVariable())
                twoFerSolution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.AssignsVariableUsingNullCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrEmptyCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpaceCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return twoFerSolution.HasComments()
                ? twoFerSolution.ApproveWithComment()
                : twoFerSolution.ContinueAnalysis();
        }

        private static SolutionAnalysis ApproveWhenOptimal(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.ApproveWhenOptimalUsingSingleLine() ||
                twoFerSolution.ApproveWhenOptimalAssigningToParameter() ||
                twoFerSolution.ApproveWhenOptimalAssigningToVariable())
                return twoFerSolution.ApproveAsOptimal();

            return twoFerSolution.ContinueAnalysis();
        }

        private static bool ApproveWhenOptimalUsingSingleLine(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.UsesSingleLine() &&
            (twoFerSolution.ReturnsStringInterpolationWithDefaultValue() ||
             twoFerSolution.ReturnsStringInterpolationWithNullCoalescingOperator());

        private static bool ApproveWhenOptimalAssigningToParameter(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsToParameter() &&
            twoFerSolution.ReturnsStringInterpolation();

        private static bool ApproveWhenOptimalAssigningToVariable(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariable() &&
            twoFerSolution.ReturnsStringInterpolationWithVariable() &&
            twoFerSolution.AssignsVariableUsingNullCoalescingOperator();
    }
}