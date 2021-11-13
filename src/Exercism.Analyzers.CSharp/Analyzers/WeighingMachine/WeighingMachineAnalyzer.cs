using System;

using Exercism.Analyzers.CSharp.Analyzers.Shared;

using static Exercism.Analyzers.CSharp.Analyzers.WeighingMachine.WeighingMachineComments;
using static Exercism.Analyzers.CSharp.Analyzers.WeighingMachine.WeighingMachineSolution;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

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
                solution.AddComment(new SolutionComment("AAAAAAAAAAAAA"));
            }

            if (!solution.TareAdjustmentIsAutoProperty)
            {
                solution.AddComment(new SolutionComment("AAAAAAAAAAAAA"));
            }

            if (solution.PrecisionPropertyHasNonPrivateSetter())
            {
                solution.AddComment(new SolutionComment("AAAAAAAAAAAAA"));
            }

            return solution.HasComments
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }

        protected override SolutionAnalysis ApproveWhenValid(WeighingMachineSolution solution)
        {
            if (solution.HasComments)
                return solution.Approve();

            return solution.ContinueAnalysis();
        }
    }
}
