using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution) =>
            Analyze(new TwoFerSolution(parsedSolution));

        private static SolutionAnalysis Analyze(TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeError() ??
            twoFerSolution.AnalyzeSingleLine() ??
            twoFerSolution.AnalyzeParameterAssignment() ??
            twoFerSolution.AnalyzeVariableAssignment() ??
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis AnalyzeError(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.UsesOverloads())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingNameMethod() ||
                twoFerSolution.InvalidNameMethod())
                return twoFerSolution.DisapproveWithComment(SharedComments.FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseDefaultValue);

            if (twoFerSolution.UsesInvalidDefaultValue())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseStringInterpolationNotStringConcat);

            return null;
        }

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.UsesSingleLine())
                return null;

            if (twoFerSolution.ReturnsStringInterpolationWithDefaultValue() ||
                twoFerSolution.ReturnsStringInterpolationWithNullCoalescingOperator())
            {
                return twoFerSolution.UsesExpressionBody() ?
                    twoFerSolution.ApproveAsOptimal() :
                    twoFerSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);
            }

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.ReturnsStringConcatenation())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.ReturnsStringFormat())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            return null;
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            if (!twoFerSolution.AssignsParameterUsingKnownExpression())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormat())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenation())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnsStringInterpolation())
                return null;

            if (twoFerSolution.AssignsParameterUsingNullCoalescingOperator())
                return twoFerSolution.ApproveWithComment(SharedComments.InlineVariable);

            if (twoFerSolution.AssignsParameterUsingNullCheck() ||
                twoFerSolution.AssignsParameterUsingIfNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            return null;
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsVariable())
                return null;

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormatWithVariable())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenationWithVariable())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnsStringInterpolationWithVariable())
                return null;

            if (twoFerSolution.AssignsVariableUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.AssignsVariableUsingNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpaceCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }
    }
}