using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.WeighingMachine.WeighingMachineSolution;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine;

internal class WeighingMachineAnalyzer : ExerciseAnalyzer<WeighingMachineSolution>
{
    protected override SolutionAnalysis AnalyzeSpecific(WeighingMachineSolution solution)
    {
        if (solution.MissingWeighingMachineClass)
            return AnalysisWithComment(MissingClass(WeighingMachineClassName));

        foreach (var missing in solution.MissingRequiredProperties())
            AddComment(MissingProperty(missing));

        if (HasComments)
            return Analysis;

        if (!solution.PrecisionIsAutoProperty)
            AddComment(PropertyIsNotAutoProperty("Precision"));

        if (!solution.TareAdjustmentIsAutoProperty)
            AddComment(PropertyIsNotAutoProperty("TareAdjustment"));

        if (solution.PrecisionPropertyHasNonPrivateSetter())
            AddComment(PropertyHasNonPrivateSetter("Precision"));

        if (!solution.WeightFieldNameIsPrivate(out var fieldName) && !string.IsNullOrWhiteSpace(fieldName))
            AddComment(UsePrivateVisibility(fieldName));

        if (!solution.IsRoundMethodCalledInDisplayWeightProperty())
            AddComment(WeighingMachineComments.RoundMethodNotCalledInDisplayWeightProperty);
                
        if (!solution.TareAdjustmentHasInitializer)
            AddComment(PropertyBetterUseInitializer("TareAdjustment"));

        return Analysis;
    }
}