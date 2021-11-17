
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine
{
    internal class WeighingMachineComments
    {
        public static SolutionComment PropertyIsNotAutoProperty(string name) =>
            new("csharp.weighingmachine.property_is_not_auto_property", new SolutionCommentParameter(Name, name));

        public static SolutionComment PrecisionPropertyHasNonPrivateSetter(string name) =>
            new("csharp.weighingmachine.property_setter_is_not_private", new SolutionCommentParameter(Name, name));

        public static SolutionComment RoundMethodNotCalledInDisplayWeightProperty =
            new("csharp.weighingmachine.round_called_in_display_weight");

        public static SolutionComment PropertyBetterUseInitializer(string name) =>
            new("csharp.weighingmachine.property_better_use_initializer", new SolutionCommentParameter(Name, name));
    }
}
