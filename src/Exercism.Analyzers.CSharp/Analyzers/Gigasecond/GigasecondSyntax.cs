using System;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondSyntax
    {
        public static ClassDeclarationSyntax Class(this ParsedSolution solution) =>
            solution.SyntaxRoot.GetClass("Gigasecond");
        
        public static MethodDeclarationSyntax AddMethod(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.GigasecondClass?.GetMethod("Add");
        
        public static ParameterSyntax AddMethodParameter(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod?.ParameterList.Parameters[0];
        
        public static InvocationExpressionSyntax AddSecondsInvocationExpression(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.DescendantNodes<InvocationExpressionSyntax>().FirstOrDefault(
                invocationExpression =>
                    invocationExpression.Expression.IsEquivalentWhenNormalized(
                        SimpleMemberAccessExpression(
                            IdentifierName(gigasecondSolution.AddMethodParameter),
                            IdentifierName("AddSeconds"))));
        
        public static ExpressionSyntax AddSecondsArgumentExpression(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsInvocationExpression?.ArgumentList.Arguments[0].Expression;
        
        public static VariableDeclaratorSyntax AddSecondsArgumentVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.GigasecondClass.AssignedVariableWithName(gigasecondSolution.AddSecondsArgumentExpression as IdentifierNameSyntax);
        
        public static ExpressionSyntax AddSecondsArgumentValueExpression(this GigasecondSolution gigasecondSolution)
        {
            switch (gigasecondSolution.AddSecondsArgumentType)
            {
                case ArgumentType.Unknown:
                    return null;
                case ArgumentType.Value:
                    return gigasecondSolution.AddSecondsArgumentExpression;
                case ArgumentType.Local:
                case ArgumentType.Field:
                    return gigasecondSolution.AddSecondsArgumentVariable.Initializer.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static AddSecondsArgumentValueType AddSecondsArgumentValueType(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.AddSecondsArgumentValueExpression.IsEquivalentWhenNormalized(GigasecondAsScientificNotation()))
                return Gigasecond.AddSecondsArgumentValueType.ScientificNotation;
            
            if (gigasecondSolution.AddSecondsArgumentValueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithSeparator()))
                return Gigasecond.AddSecondsArgumentValueType.DigitsWithSeparator;
            
            if (gigasecondSolution.AddSecondsArgumentValueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithoutSeparator()))
                return Gigasecond.AddSecondsArgumentValueType.DigitsWithoutSeparator;
            
            if (gigasecondSolution.AddSecondsArgumentValueExpression.IsEquivalentWhenNormalized(GigasecondAsMathPowInvocationExpression()))
                return Gigasecond.AddSecondsArgumentValueType.MathPow;

            return Gigasecond.AddSecondsArgumentValueType.Unknown;
        }

        public static ReturnType AddSecondsReturnedAs(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.AddSecondsInvocationExpression.AssignedToVariable(out var variableDeclarator))
                return gigasecondSolution.AddMethodReturnedExpression.IsEquivalentWhenNormalized(IdentifierName(variableDeclarator))
                    ? ReturnType.VariableAssignment
                    : ReturnType.Unknown;

            if (gigasecondSolution.AddSecondsInvocationExpression.AssignedToParameter(gigasecondSolution.AddMethodParameter))
                return gigasecondSolution.AddMethodReturnedExpression.IsEquivalentWhenNormalized(IdentifierName(gigasecondSolution.AddMethodParameter))
                    ? ReturnType.ParameterAssigment
                    : ReturnType.Unknown;

            return gigasecondSolution.AddMethodReturnedExpression.IsEquivalentWhenNormalized(gigasecondSolution.AddSecondsInvocationExpression)
                ? ReturnType.ImmediateValue
                : ReturnType.Unknown;
        }

        public static ArgumentType AddSecondsArgumentDefinedAs(this GigasecondSolution gigasecondSolution)
        {
            if (gigasecondSolution.AddSecondsArgumentVariableFieldDeclaration != null)
                return ArgumentType.Field;
            
            if (gigasecondSolution.AddSecondsArgumentVariableLocalDeclarationStatement != null)
                return ArgumentType.Local;

            if (gigasecondSolution.AddSecondsArgumentExpression != null)
                return ArgumentType.Value;

            return ArgumentType.Unknown;
        }

        private static bool AssignedToVariable(this InvocationExpressionSyntax addSecondsInvocationExpression, out VariableDeclaratorSyntax variableDeclarator)
        {
            variableDeclarator =
                addSecondsInvocationExpression != null &&
                addSecondsInvocationExpression.Parent is EqualsValueClauseSyntax equalsValueClause
                ? equalsValueClause.Parent as VariableDeclaratorSyntax
                : null;

            return variableDeclarator != null;
        }

        private static bool AssignedToParameter(this InvocationExpressionSyntax addSecondsInvocationExpression, ParameterSyntax addMethodParameter) =>
            addSecondsInvocationExpression != null &&
            addSecondsInvocationExpression.Parent is AssignmentExpressionSyntax assignmentExpression
            && assignmentExpression.Left.IsEquivalentWhenNormalized(IdentifierName(addMethodParameter));

        public static bool UsesScientificNotation(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentValueType == Gigasecond.AddSecondsArgumentValueType.ScientificNotation;

        public static bool UsesDigitsWithoutSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentValueType == Gigasecond.AddSecondsArgumentValueType.DigitsWithoutSeparator;

        public static bool UsesDigitsWithSeparator(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentValueType == Gigasecond.AddSecondsArgumentValueType.DigitsWithSeparator;

        public static bool UsesMathPow(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentValueType == Gigasecond.AddSecondsArgumentValueType.MathPow;
        
        public static bool AssignsToParameterAndReturns(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsReturnType == ReturnType.ParameterAssigment;
        
        public static bool AssignsToVariableAndReturns(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsReturnType == ReturnType.VariableAssignment;

        public static bool UsesAddSecondsWithFieldVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentType == ArgumentType.Field;
        
        public static bool UsesLocalVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentType == ArgumentType.Local;
        
        public static bool UsesLocalConstVariable(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesLocalVariable() &&
            gigasecondSolution.AddSecondsArgumentVariableLocalDeclarationStatement.IsConst;

        public static bool UsesField(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsArgumentType == ArgumentType.Field;

        public static bool UsesPrivateField(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesField() &&
            gigasecondSolution.AddSecondsArgumentVariableFieldDeclaration.IsPrivate();

        public static bool UsesConstField(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesField() &&
            gigasecondSolution.AddSecondsArgumentVariableFieldDeclaration.IsConst();

        public static bool UsesPrivateConstField(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.UsesPrivateField() &&
            gigasecondSolution.UsesConstField();
        
        public static bool UsesExpressionBody(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.IsExpressionBody();

        public static bool DoesNotUseAddSeconds(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddSecondsInvocationExpression == null;

        public static bool CreatesNewDatetime(this GigasecondSolution gigasecondSolution) =>
            gigasecondSolution.AddMethod.CreatesObjectOfType<DateTime>();
    }
}