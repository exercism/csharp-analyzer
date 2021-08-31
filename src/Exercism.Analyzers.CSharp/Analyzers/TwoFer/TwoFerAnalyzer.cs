using Exercism.Analyzers.CSharp.Analyzers.Shared;

using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSolution;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal class TwoFerAnalyzer : SharedAnalyzer<TwoFerSolution>
    {
        protected override SolutionAnalysis DisapproveWhenInvalid(TwoFerSolution solution)
        {
            if (solution.MissingTwoFerClass)
                solution.AddComment(MissingClass(TwoFerClassName));

            if (solution.MissingSpeakMethod)
                solution.AddComment(MissingMethod(SpeakMethodName));

            if (solution.InvalidSpeakMethod)
                solution.AddComment(InvalidMethodSignature(SpeakMethodName, SpeakMethodSignature));

            if (solution.UsesOverloads)
                solution.AddComment(UseDefaultValueNotOverloads);

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

        protected override SolutionAnalysis ApproveWhenValid(TwoFerSolution solution)
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
                return AnalyzeSingleLine(solution);

            if (solution.AssignsToParameter)
                return AnalyzeParameterAssignment(solution);

            if (solution.AssignsVariable)
                return AnalyzeVariableAssignment(solution);

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeSingleLine(TwoFerSolution solution)
        {
            if (!solution.UsesExpressionBody)
                solution.AddComment(UseExpressionBodiedMember(SpeakMethodName));

            if (solution.UsesStringInterpolationWithDefaultValue ||
                solution.UsesStringInterpolationWithNullCoalescingOperator ||
                solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }

        private static SolutionAnalysis AnalyzeParameterAssignment(TwoFerSolution solution)
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

        private static SolutionAnalysis AnalyzeVariableAssignment(TwoFerSolution solution)
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