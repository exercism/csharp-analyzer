using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSolution;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;

internal class TwoFerAnalyzer : ExerciseAnalyzer<TwoFerSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(TwoFerSolution solution)
    {
        if (solution.MissingTwoFerClass)
            return AnalysisWithComment(MissingClass(TwoFerClassName));

        if (solution.MissingSpeakMethod)
            return AnalysisWithComment(MissingMethod(SpeakMethodName));

        if (solution.InvalidSpeakMethod)
            return AnalysisWithComment(InvalidMethodSignature(SpeakMethodName, SpeakMethodSignature));

        if (solution.UsesOverloads)
            return AnalysisWithComment(UseDefaultValueNotOverloads);

        if (solution.UsesDuplicateString)
            AddComment(UseSingleFormattedStringNotMultiple);

        if (solution.NoDefaultValue)
            AddComment(UseDefaultValue(solution.SpeakMethodParameterName));

        if (solution.UsesInvalidDefaultValue)
            AddComment(InvalidDefaultValue(solution.SpeakMethodParameterName, solution.SpeakMethodParameterDefaultValue));

        if (solution.UsesStringReplace)
            AddComment(UseStringInterpolationNotStringReplace);

        if (solution.UsesStringJoin)
            AddComment(UseStringInterpolationNotStringJoin);

        if (solution.UsesStringConcat)
            AddComment(UseStringInterpolationNotStringConcat);

        if (solution.UsesStringConcatenation)
            AddComment(UseStringInterpolationNotStringConcatenation);

        if (solution.UsesStringFormat)
            AddComment(UseStringInterpolationNotStringFormat);

        if (solution.UsesIsNullOrEmptyCheck)
            AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);

        if (solution.UsesIsNullOrWhiteSpaceCheck)
            AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);

        if (solution.UsesNullCheck)
            AddComment(UseNullCoalescingOperatorNotNullCheck);

        if (solution.CanUseExpressionBody)
            AddComment(UseExpressionBodiedMember(SpeakMethodName));
        
        if (!solution.AssignsParameterUsingKnownExpression)
            return Analysis;

        if (solution.AssignsParameterUsingNullCoalescingOperator)
            AddComment(DoNotAssignAndReturn);

        return Analysis;
    }
}