using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSolution;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;

internal class TwoFerAnalyzer : ExerciseAnalyzer<TwoFerSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(TwoFerSolution solution)
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

        if (!solution.UsesExpressionBody)
            solution.AddComment(UseExpressionBodiedMember(SpeakMethodName));
        
        if (!solution.AssignsParameterUsingKnownExpression)
            return solution.Analysis;

        if (solution.AssignsParameterUsingNullCoalescingOperator)
            solution.AddComment(DoNotAssignAndReturn);

        return solution.Analysis;
    }
}