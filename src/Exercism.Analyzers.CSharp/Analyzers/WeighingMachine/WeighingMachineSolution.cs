using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exercism.Analyzers.CSharp.Syntax;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine
{
    internal class WeighingMachineSolution : Solution
    {
        public const string WeighingMachineClassName = "WeighingMachine";
        public const string PrecisionPropertyName = "Precision";
        public const string WeightPropertyName = "Weight";
        public const string TareAdjustmentPropertyName = "TareAdjustment";
        public const string DisplayWeightPropertyName = "DisplayWeight";
        public const string AddMethodName = "Add";
        public const string AddMethodSignature = "public static DateTime Add(DateTime moment)";

        public WeighingMachineSolution(Solution solution) : base(solution)
        {
        }

        private ClassDeclarationSyntax WeighingMachineClass => SyntaxRoot.GetClass(WeighingMachineClassName);

        public bool MissingWeighingMachineClass => WeighingMachineClass is null;

        private PropertyDeclarationSyntax PrecisionProperty => WeighingMachineClass?.GetProperty(PrecisionPropertyName);

        private PropertyDeclarationSyntax WeightProperty => WeighingMachineClass?.GetProperty(WeightPropertyName);

        private PropertyDeclarationSyntax TareAdjustmentProperty => WeighingMachineClass?.GetProperty(TareAdjustmentPropertyName);

        private PropertyDeclarationSyntax DisplayWeightProperty => WeighingMachineClass?.GetProperty(DisplayWeightPropertyName);

        public IEnumerable<string> MissingRequiredProperties()
        {
            if (PrecisionProperty is null) yield return PrecisionPropertyName;
            if (WeightProperty is null) yield return WeightPropertyName;
            if (TareAdjustmentProperty is null) yield return TareAdjustmentPropertyName;
            if (DisplayWeightProperty is null) yield return DisplayWeightPropertyName;
        }
    }
}
