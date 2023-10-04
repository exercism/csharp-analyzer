using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class WeighingMachineAnalyzer : Analyzer
{
    public WeighingMachineAnalyzer(Submission submission) : base(submission)
    {
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        switch (GetDeclaredSymbolName(node))
        {
            case "WeighingMachine.DisplayWeight":
            {
                SyntaxNode getter = node.ExpressionBody == null
                    ? node.DescendantNodes()
                        .OfType<AccessorDeclarationSyntax>()
                        .FirstOrDefault(
                            accessorDeclaration => accessorDeclaration.IsKind(SyntaxKind.GetAccessorDeclaration))
                    : node.ExpressionBody.Expression;
        
                if (getter is not null &&
                    getter.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Select(GetSymbolName)
                        .All(symbolName => symbolName != "System.Math.Round(double, double)"))
                    AddComment(Comments.UseMathRoundInDisplayWeight);
                break;
            }
            case "WeighingMachine.Precision":
                if (IsNotAutoImplementedProperty(node))
                    AddComment(Comments.PropertyIsNotAutoProperty(node.Identifier.Text));
                break;
            case "WeighingMachine.TareAdjustment":
                if (IsNotAutoImplementedProperty(node))
                    AddComment(Comments.PropertyIsNotAutoProperty(node.Identifier.Text));
                break;
        }
        
        base.VisitPropertyDeclaration(node);
    }

    private static bool IsNotAutoImplementedProperty(PropertyDeclarationSyntax node)
    {
        var accessorDeclarations = node.DescendantNodes().OfType<AccessorDeclarationSyntax>().ToArray();
        var getAccessor = accessorDeclarations.FirstOrDefault(accessorDeclaration =>
            accessorDeclaration.IsKind(SyntaxKind.GetAccessorDeclaration));
        var setAccessor = accessorDeclarations.FirstOrDefault(accessorDeclaration =>
            accessorDeclaration.IsKind(SyntaxKind.SetAccessorDeclaration));
        return getAccessor is not null && (getAccessor.Body is not null || getAccessor.ExpressionBody is not null) ||
               setAccessor is not null && (setAccessor.Body is not null || setAccessor.ExpressionBody is not null);
    }

    private static class Comments
    {
        public static Comment UseMathRoundInDisplayWeight =
            new("csharp.weighing-machine.use_math_round_in_display_weight", CommentType.Essential);
        
        public static Comment PropertyIsNotAutoProperty(string name) =>
            new("csharp.general.property_is_not_auto_property", CommentType.Actionable,
                new CommentParameter("name", name));
    }
}