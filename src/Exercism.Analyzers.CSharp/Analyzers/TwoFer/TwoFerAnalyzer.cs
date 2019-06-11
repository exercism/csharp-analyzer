using System.Collections.Generic;
using System.Linq;
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
            twoFerSolution.ReferToMentor();

        private static SolutionAnalysis DisapproveWhenInvalid(this TwoFerSolution twoFerSolution)
        {
            var comments = new List<string>();

            if (twoFerSolution.UsesOverloads)
                comments.Add(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingSpeakMethod ||
                twoFerSolution.InvalidSpeakMethod)
                comments.Add(FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString)
                comments.Add(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue)
                comments.Add(UseDefaultValue);

            if (twoFerSolution.InvalidDefaultValue)
                comments.Add(InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace)
                comments.Add(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin)
                comments.Add(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat)
                comments.Add(UseStringInterpolationNotStringConcat);

            return comments.Any() ? twoFerSolution.DisapproveWithComment(comments.ToArray()) : null;
        }

        private static SolutionAnalysis ApproveWhenValid(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AnalyzeSingleLine() ??
            twoFerSolution.AnalyzeParameterAssignment() ??
            twoFerSolution.AnalyzeVariableAssignment();

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.UsesSingleLine())
                return null;

            var comments = new List<string>();

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrEmptyCheck())
                comments.Add(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck())
                comments.Add(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithNullCheck())
                comments.Add(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.ReturnsStringConcatenation())
                comments.Add(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.ReturnsStringFormat())
                comments.Add(UseStringInterpolationNotStringFormat);

            if (!twoFerSolution.UsesExpressionBody())
                comments.Add(UseExpressionBodiedMember);

            if (comments.Any())
                return twoFerSolution.ApproveWithComment(comments.ToArray());

            if (twoFerSolution.ReturnsStringInterpolationWithDefaultValue() ||
                twoFerSolution.ReturnsStringInterpolationWithNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            return null;
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsToParameter())
                return null;

            var comments = new List<string>();

            if (!twoFerSolution.AssignsParameterUsingKnownExpression())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormat())
                comments.Add(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenation())
                comments.Add(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.AssignsParameterUsingNullCoalescingOperator())
                comments.Add(ReturnImmediately);

            if (twoFerSolution.AssignsParameterUsingNullCheck() ||
                twoFerSolution.AssignsParameterUsingIfNullCheck())
                comments.Add(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrEmptyCheck())
                comments.Add(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck())
                comments.Add(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (comments.Any())
                return twoFerSolution.ApproveWithComment(comments.ToArray());

            if (twoFerSolution.ReturnsStringInterpolation())
                return twoFerSolution.ApproveAsOptimal();

            return null;
        }

        private static SolutionAnalysis AnalyzeVariableAssignment(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.AssignsVariable())
                return null;

            var comments = new List<string>();

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnsStringFormatWithVariable())
                comments.Add(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnsStringConcatenationWithVariable())
                comments.Add(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.AssignsVariableUsingNullCheck())
                comments.Add(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrEmptyCheck())
                comments.Add(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpaceCheck())
                comments.Add(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (comments.Any())
                return twoFerSolution.ApproveWithComment(comments.ToArray());

            if (twoFerSolution.ReturnsStringInterpolationWithVariable() &&
                twoFerSolution.AssignsVariableUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            return null;
        }
    }
}