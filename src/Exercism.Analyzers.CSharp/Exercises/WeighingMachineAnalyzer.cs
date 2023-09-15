namespace Exercism.Analyzers.CSharp.Exercises;

internal class WeighingMachineAnalyzer : ExerciseAnalyzer
{
    protected override void AnalyzeExerciseSpecific(Solution solution)
    {
        // TODO
    }
    
    private static class Comments
    {
        public static Comment RoundMethodNotCalledInDisplayWeightProperty =
            new("csharp.weighingmachine.round_called_in_display_weight", CommentType.Essential);
    }
}

// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
// using static Exercism.Analyzers.CSharp.Analyzers.WeighingMachine.WeighingMachineSolution;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine;
//
// internal class WeighingMachineAnalyzer : ExerciseAnalyzer<WeighingMachineSolution>
// {
//     protected override SolutionAnalysis AnalyzeSpecific(WeighingMachineSolution solution)
//     {
//         if (solution.MissingWeighingMachineClass)
//             return AnalysisWithComment(MissingClass(WeighingMachineClassName));
//
//         foreach (var missing in solution.MissingRequiredProperties())
//             AddComment(MissingProperty(missing));
//
//         if (HasComments)
//             return Analysis;
//
//         if (!solution.PrecisionIsAutoProperty)
//             AddComment(PropertyIsNotAutoProperty("Precision"));
//
//         if (!solution.TareAdjustmentIsAutoProperty)
//             AddComment(PropertyIsNotAutoProperty("TareAdjustment"));
//
//         if (solution.PrecisionPropertyHasNonPrivateSetter())
//             AddComment(PropertyHasNonPrivateSetter("Precision"));
//
//         if (!solution.WeightFieldNameIsPrivate(out var fieldName) && !string.IsNullOrWhiteSpace(fieldName))
//             AddComment(UsePrivateVisibility(fieldName));
//
//         if (!solution.IsRoundMethodCalledInDisplayWeightProperty())
//             AddComment(WeighingMachineComments.RoundMethodNotCalledInDisplayWeightProperty);
//                 
//         if (!solution.TareAdjustmentHasInitializer)
//             AddComment(PropertyBetterUseInitializer("TareAdjustment"));
//
//         return Analysis;
//     }
// }
//
//
// namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine;
//

//
// using System.Collections.Generic;
// using System.Linq;
//
// using Exercism.Analyzers.CSharp.Syntax;
// using Exercism.Analyzers.CSharp.Syntax.Comparison;
//
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine;
//
// internal class WeighingMachineSolution : Solution
// {
//     public const string WeighingMachineClassName = "WeighingMachine";
//     public const string PrecisionPropertyName = "Precision";
//     public const string WeightPropertyName = "Weight";
//     public const string TareAdjustmentPropertyName = "TareAdjustment";
//     public const string DisplayWeightPropertyName = "DisplayWeight";
//
//     public WeighingMachineSolution(Solution solution) : base(solution)
//     {
//     }
//
//     private ClassDeclarationSyntax WeighingMachineClass => SyntaxRoot.GetClass(WeighingMachineClassName);
//
//     public bool MissingWeighingMachineClass => WeighingMachineClass is null;
//
//     private PropertyDeclarationSyntax PrecisionProperty => WeighingMachineClass?.GetProperty(PrecisionPropertyName);
//
//     private PropertyDeclarationSyntax WeightProperty => WeighingMachineClass?.GetProperty(WeightPropertyName);
//
//     private PropertyDeclarationSyntax TareAdjustmentProperty => WeighingMachineClass?.GetProperty(TareAdjustmentPropertyName);
//
//     private PropertyDeclarationSyntax DisplayWeightProperty => WeighingMachineClass?.GetProperty(DisplayWeightPropertyName);
//
//     public IEnumerable<string> MissingRequiredProperties()
//     {
//         if (PrecisionProperty is null) yield return PrecisionPropertyName;
//         if (WeightProperty is null) yield return WeightPropertyName;
//         if (TareAdjustmentProperty is null) yield return TareAdjustmentPropertyName;
//         if (DisplayWeightProperty is null) yield return DisplayWeightPropertyName;
//     }
//
//     public bool PrecisionIsAutoProperty => PrecisionProperty?.IsAutoProperty() == true;
//
//     public bool TareAdjustmentIsAutoProperty => TareAdjustmentProperty?.IsAutoProperty() == true;
//
//     public bool TareAdjustmentHasInitializer => TareAdjustmentProperty?.HasInitializer() == true;
//
//     public bool PrecisionPropertyHasNonPrivateSetter()
//     {
//         var setAccessor = PrecisionProperty.GetSetAccessor();
//         return setAccessor is not null &&
//                (setAccessor.Modifiers.Count == 0 ||
//                 !setAccessor.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PrivateKeyword)));
//     }
//
//     public bool WeightFieldNameIsPrivate(out string fieldName)
//     {
//         fieldName = WeightProperty.GetBakingFieldName();
//
//         if (string.IsNullOrEmpty(fieldName))
//         {
//             return false;
//         }
//
//         return WeighingMachineClass?.GetField(fieldName)?.IsPrivate() ?? false;
//     }
//
//     public bool IsRoundMethodCalledInDisplayWeightProperty() =>
//         DisplayWeightProperty?.GetMethodCalled("Round") is not null;
// }