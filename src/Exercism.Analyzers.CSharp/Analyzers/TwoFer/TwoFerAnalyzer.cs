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
            if (twoFerSolution.UsesOverloads)
                twoFerSolution.AddComment(UseDefaultValueNotOverloads);

            if (twoFerSolution.MissingSpeakMethod ||
                twoFerSolution.InvalidSpeakMethod)
                twoFerSolution.AddComment(FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString)
                twoFerSolution.AddComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue)
                twoFerSolution.AddComment(UseDefaultValue(twoFerSolution.SpeakMethodParameterName));

            if (twoFerSolution.InvalidDefaultValue)
                twoFerSolution.AddComment(InvalidDefaultValue(twoFerSolution.SpeakMethodParameterName, twoFerSolution.SpeakMethodParameterDefaultValue));

            if (twoFerSolution.UsesStringReplace)
                twoFerSolution.AddComment(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin)
                twoFerSolution.AddComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat)
                twoFerSolution.AddComment(UseStringInterpolationNotStringConcat);

            return twoFerSolution.HasComments()
                ? twoFerSolution.Disapprove()
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
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithNullCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.ReturnsStringConcatenation())
                twoFerSolution.AddComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.ReturnsStringFormat())
                twoFerSolution.AddComment(UseStringInterpolationNotStringFormat);

            if (!twoFerSolution.UsesExpressionBody())
                twoFerSolution.AddComment(UseExpressionBodiedMember(twoFerSolution.SpeakMethodName));

            if (twoFerSolution.ReturnsStringInterpolationWithDefaultValue() ||
                twoFerSolution.ReturnsStringInterpolationWithNullCoalescingOperator() ||
                twoFerSolution.HasComments())
                return twoFerSolution.Approve();

            return twoFerSolution.ContinueAnalysis();
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
                twoFerSolution.AddComment(DoNotAssignAndReturn);

            if (twoFerSolution.AssignsParameterUsingNullCheck() ||
                twoFerSolution.AssignsParameterUsingIfNullCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrEmptyCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrEmptyCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
                twoFerSolution.AssignsParameterUsingIfIsNullOrWhiteSpaceCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolation() ||
                twoFerSolution.HasComments())
                return twoFerSolution.Approve();

            return twoFerSolution.ContinueAnalysis();
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
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotNullCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrEmptyCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

            if (twoFerSolution.AssignsVariableUsingIsNullOrWhiteSpaceCheck())
                twoFerSolution.AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnsStringInterpolationWithVariable() &&
                twoFerSolution.AssignsVariableUsingNullCoalescingOperator() ||
                twoFerSolution.HasComments())
                return twoFerSolution.Approve();

            return twoFerSolution.ContinueAnalysis();
        }
    }
}