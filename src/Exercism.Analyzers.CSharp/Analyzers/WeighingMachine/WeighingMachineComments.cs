using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine
{
    internal class WeighingMachineComments
    {
        public static SolutionComment PropertyIsNotAutoProperty(string name) =>
            new SolutionComment("csharp.weighingmachine.property_is_not_auto_property", new SolutionCommentParameter(Name, name));

        public static SolutionComment PrecisionPropertyHasNonPrivateSetter(string name) =>
            new SolutionComment("csharp.weighingmachine.property_setter_is_not_private", new SolutionCommentParameter(Name, name));

        public static SolutionComment RoundMethodNotCalledInDisplayWeightProperty =
            new SolutionComment("csharp.weighingmachine.round_called_in_display_weight");
    }
}
