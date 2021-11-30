
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine
{
    internal class WeighingMachineComments
    {
        public static SolutionComment RoundMethodNotCalledInDisplayWeightProperty =
            new("csharp.weighingmachine.round_called_in_display_weight");
    }
}
