using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new TwoFerSolution(parsedSolution));

        private static SolutionAnalysis Analyze(TwoFerSolution twoFerSolution) =>
            twoFerSolution.DisapproveWhenInvalid() ??
            twoFerSolution.ApproveWhenValid() ??
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.UsesOverloads())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingSpeakMethod() ||
                twoFerSolution.InvalidSpeakMethod())
                return twoFerSolution.DisapproveWithComment(FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue())
                return twoFerSolution.DisapproveWithComment(UseDefaultValue);

            if (twoFerSolution.UsesInvalidDefaultValue())
                return twoFerSolution.DisapproveWithComment(InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringConcat);

            return null;
        }

        private static SolutionAnalysis ApproveWhenValid(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeSingleLine() ??
            twoFerSolution.AnalyzeParameterAssignment() ??
            twoFerSolution.AnalyzeVariableAssignment();

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.UsesSingleLine())
                return null;

            if (twoFerSolution.ReturnsStringInterpolationWithDefaultValue() ||
                twoFerSolution.ReturnsStringInterpolationWithNullCoalescingOperator())
            {
                return twoFerSolution.UsesExpressionBody() ?
                    twoFerSolution.ApproveAsOptimal() :
                    twoFerSolution.ApproveWithComment(UseExpressionBodiedMember);
            }

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithNullCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.ReturnsStringConcatenation())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.ReturnsStringFormat())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            return null;
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            if (!twoFerSolution.AssignsParameterUsingKnownExpression())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormat())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenation())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnsStringInterpolation())
                return null;

            if (twoFerSolution.AssignsParameterUsingNullCoalescingOperator())
                return twoFerSolution.ApproveWithComment(ReturnImmediately);

            if (twoFerSolution.AssignsParameterUsingNullCheck() ||
                twoFerSolution.AssignsParameterUsingIfNullCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            return null;
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsVariable())
                return null;

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormatWithVariable())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenationWithVariable())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnsStringInterpolationWithVariable())
                return null;

            if (twoFerSolution.AssignsVariableUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.AssignsVariableUsingNullCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }
    }
}