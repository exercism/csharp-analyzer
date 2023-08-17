using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSolution;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond;

internal class GigasecondAnalyzer : ExerciseAnalyzer<GigasecondSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(GigasecondSolution solution)
    {
        if (solution.MissingGigasecondClass)
            return AnalysisWithComment(MissingClass(GigasecondClassName));

        if (solution.MissingAddMethod)
            return AnalysisWithComment(MissingMethod(AddMethodName));

        if (solution.InvalidAddMethod)
            return AnalysisWithComment(InvalidMethodSignature(AddMethodName, AddMethodSignature));

        if (solution.CreatesNewDatetime)
            AddComment(DoNotCreateDateTime);

        if (solution.DoesNotUseAddSeconds)
            AddComment(UseAddSeconds);

        if (solution.UsesMathPow)
            AddComment(UseScientificNotationNotMathPow(solution.GigasecondValue));

        if (solution.UsesDigitsWithoutSeparator)
            AddComment(UseScientificNotationOrDigitSeparators(solution.GigasecondValue));

        if (solution.AssignsToParameterAndReturns ||
            solution.AssignsToVariableAndReturns)
            AddComment(DoNotAssignAndReturn);

        if (solution.UsesLocalVariable &&
            !solution.UsesLocalConstVariable)
            AddComment(ConvertVariableToConst(solution.GigasecondValueVariableName));

        if (solution.UsesField &&
            !solution.UsesConstField)
            AddComment(ConvertFieldToConst(solution.GigasecondValueFieldName));

        if (solution.UsesField &&
            !solution.UsesPrivateField)
            AddComment(UsePrivateVisibility(solution.GigasecondValueFieldName));

        if (solution.CanUseExpressionBody)
            AddComment(UseExpressionBodiedMember(AddMethodName));

        return Analysis;
    }
}