using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondComments;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSolution;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond;

internal class GigasecondAnalyzer : ExerciseAnalyzer<GigasecondSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(GigasecondSolution solution)
    {
        if (solution.MissingGigasecondClass)
            return solution.AnalysisWithComment(MissingClass(GigasecondClassName));

        if (solution.MissingAddMethod)
            return solution.AnalysisWithComment(MissingMethod(AddMethodName));

        if (solution.InvalidAddMethod)
            return solution.AnalysisWithComment(InvalidMethodSignature(AddMethodName, AddMethodSignature));

        if (solution.CreatesNewDatetime)
            solution.AddComment(DoNotCreateDateTime);

        if (solution.DoesNotUseAddSeconds)
            solution.AddComment(UseAddSeconds);

        if (solution.UsesMathPow)
            solution.AddComment(UseScientificNotationNotMathPow(solution.GigasecondValue));

        if (solution.UsesDigitsWithoutSeparator)
            solution.AddComment(UseScientificNotationOrDigitSeparators(solution.GigasecondValue));

        if (solution.AssignsToParameterAndReturns ||
            solution.AssignsToVariableAndReturns)
            solution.AddComment(DoNotAssignAndReturn);

        if (solution.UsesLocalVariable &&
            !solution.UsesLocalConstVariable)
            solution.AddComment(ConvertVariableToConst(solution.GigasecondValueVariableName));

        if (solution.UsesField &&
            !solution.UsesConstField)
            solution.AddComment(ConvertFieldToConst(solution.GigasecondValueFieldName));

        if (solution.UsesField &&
            !solution.UsesPrivateField)
            solution.AddComment(UsePrivateVisibility(solution.GigasecondValueFieldName));

        if (solution.UsesSingleLine &&
            !solution.UsesExpressionBody)
            solution.AddComment(UseExpressionBodiedMember(AddMethodName));

        return solution.Analysis;
    }
}