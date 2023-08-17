using Exercism.Analyzers.CSharp.Analyzers.Shared;

using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.WeighingMachine.WeighingMachineSolution;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine;

internal class WeighingMachineAnalyzer : ExerciseAnalyzer<WeighingMachineSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(WeighingMachineSolution solution)
    {
        if (solution.MissingWeighingMachineClass)
            solution.AddComment(MissingClass(WeighingMachineClassName));

        foreach (var missing in solution.MissingRequiredProperties())
            solution.AddComment(MissingProperty(missing));

        if (!solution.PrecisionIsAutoProperty)
            solution.AddComment(SharedComments.PropertyIsNotAutoProperty("Precision"));

        if (!solution.TareAdjustmentIsAutoProperty)
            solution.AddComment(SharedComments.PropertyIsNotAutoProperty("TareAdjustment"));

        if (solution.PrecisionPropertyHasNonPrivateSetter())
            solution.AddComment(SharedComments.PropertyHasNonPrivateSetter("Precision"));

        if (!solution.WeightFieldNameIsPrivate(out var fieldName) && !string.IsNullOrWhiteSpace(fieldName))
            solution.AddComment(UsePrivateVisibility(fieldName));

        if (!solution.IsRoundMethodCalledInDisplayWeightProperty())
            solution.AddComment(WeighingMachineComments.RoundMethodNotCalledInDisplayWeightProperty);
                
        if (!solution.TareAdjustmentHasInitializer)
            solution.AddComment(SharedComments.PropertyBetterUseInitializer("TareAdjustment"));

        return solution.Analysis;
    }
}