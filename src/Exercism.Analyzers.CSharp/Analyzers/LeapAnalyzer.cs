using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Exercism.Analyzers.CSharp.Analyzers;

internal class LeapAnalyzer : Analyzer
{
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (SemanticModel.GetDeclaredSymbol(node)?.ToString() == "Leap.IsLeapYear(int)")
        {
            var symbol = SemanticModel.GetDeclaredSymbol(node.ParameterList.Parameters[0]);
            if (symbol != null)
            {
                foreach (var references in SymbolFinder.FindReferencesAsync(symbol, Project.Solution).GetAwaiter()
                             .GetResult())
                {
                    if (references.Locations.Count() > 3)
                        AddComment(Comments.UseMinimumNumberOfChecks);
                }
            }   
        }

        base.VisitMethodDeclaration(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        if (SemanticModel.GetSymbolInfo(node).Symbol?.ToString() == "System.DateTime.IsLeapYear(int)")
            AddComment(Comments.DoNotUseIsLeapYear);

        base.VisitInvocationExpression(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        AddComment(Comments.DoNotUseIfStatement);
        
        base.VisitIfStatement(node);
    }

    private static class Comments
    {
        public static readonly Comment DoNotUseIsLeapYear =
            new("csharp.leap.do_not_use_is_leap_year", CommentType.Essential);

        public static readonly Comment DoNotUseIfStatement =
            new("csharp.leap.do_not_use_if_statement", CommentType.Actionable);

        public static readonly Comment UseMinimumNumberOfChecks =
            new("csharp.leap.use_minimum_number_of_checks", CommentType.Actionable);
    }
}
