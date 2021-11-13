
using Exercism.Analyzers.CSharp.Analyzers.Shared;

using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.WeighingMachine.WeighingMachineSolution;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine
{
    internal class WeighingMachineAnalyzer : SharedAnalyzer<WeighingMachineSolution>
    {
        protected override SolutionAnalysis DisapproveWhenInvalid(WeighingMachineSolution solution)
        {
            if (solution.MissingWeighingMachineClass)
                solution.AddComment(MissingClass(WeighingMachineClassName));

            foreach (var missing in solution.MissingRequiredProperties())
            {
                solution.AddComment(MissingProperty(missing));
            }

            if (!solution.PrecisionIsAutoProperty)
            {
                solution.AddComment(new SolutionComment("not PrecisionIsAutoProperty"));
            }

            if (!solution.TareAdjustmentIsAutoProperty)
            {
                solution.AddComment(new SolutionComment("not TareAdjustmentIsAutoProperty"));
            }

            if (solution.PrecisionPropertyHasNonPrivateSetter())
            {
                solution.AddComment(new SolutionComment("PrecisionPropertyHasNonPrivateSetter"));
            }

            if (!solution.WeightFieldNameIsPrivate)
            {
                solution.AddComment(new SolutionComment("not WeightFieldNameIsPrivate"));
            }

            if (!solution.IsRoundMethodCalledInDisplayWeightProperty())
            {
                solution.AddComment(new SolutionComment("not IsRoundMethodCalledInDisplayWeightProperty"));
            }

            return solution.HasComments
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }

        protected override SolutionAnalysis ApproveWhenValid(WeighingMachineSolution solution)
        {
            if (solution.TareAdjustmentHasInitializer)
            {
                solution.AddComment(new SolutionComment("TareAdjustmentHasInitializer"));
            }

            if (solution.HasComments)
            {
                return solution.Approve();
            }

            return solution.ContinueAnalysis();
        }
    }
}
