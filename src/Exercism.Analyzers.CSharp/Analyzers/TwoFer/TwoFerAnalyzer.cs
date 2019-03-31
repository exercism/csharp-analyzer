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

            if (twoFerSolution.UsesDefaultInterpolatedStringExpression() ||
                twoFerSolution.UsesNullCoalescingInInterpolatedStringExpression())
            {
                return twoFerSolution.UsesExpressionBody() ?
                    twoFerSolution.ApproveAsOptimal() :
                    twoFerSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);
            }

            if (twoFerSolution.UsesIsNullOrEmptyInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.UsesIsNullOrWhiteSpaceInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.UsesTernaryOperatorInInterpolatedStringExpression())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (TwoFerSyntax.UsesStringConcatenation(twoFerSolution))
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (TwoFerSyntax.UsesStringFormat(twoFerSolution))
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            return null;
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            if (!twoFerSolution.ParameterAssignedUsingKnownExpression())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.UsesStringFormat())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            if (twoFerSolution.UsesStringConcatenation())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.UsesStringInterpolation())
                return null;

            if (twoFerSolution.ParameterAssignedUsingNullCoalescingExpression())
                return twoFerSolution.ApproveWithComment(SharedComments.InlineVariable);

            if (twoFerSolution.ParameterAssignedUsingNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.ParameterAssignedUsingIsNullOrEmptyCheck())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.ParameterAssignedUsingIsNullOrWhiteSpaceCheck())
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

            if (twoFerSolution.VariableAssignedUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.VariableAssignedUsingNullCheck())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrEmpty())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }
    }
}