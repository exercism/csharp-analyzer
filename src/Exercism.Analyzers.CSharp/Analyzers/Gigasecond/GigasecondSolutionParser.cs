using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal static class GigasecondSolutionParser
    {
        public static GigasecondSolution Parse(ParsedSolution solution)
        {
            var gigasecondClass = solution.GigasecondClass();
            var addMethod = gigasecondClass.AddMethod();
            var addMethodParameter = addMethod.FirstParameter();
            var addMethodReturnedExpression = addMethod.ReturnedExpression();
            var addSecondsInvocationExpression = addMethod.AddSecondsInvocationExpression(addMethodParameter);
            var addSecondsArgumentExpression = addSecondsInvocationExpression.FirstArgumentExpression();
            var addSecondsArgumentVariable = gigasecondClass.ArgumentVariable(addSecondsArgumentExpression);
            var addSecondsArgumentVariableFieldDeclaration = addSecondsArgumentVariable.FieldDeclaration();
            var addSecondsArgumentVariableLocalDeclarationStatement = addSecondsArgumentVariable.LocalDeclarationStatement();
            var addSecondsArgumentType = ArgumentDefinedAs(addSecondsArgumentVariableFieldDeclaration, addSecondsArgumentVariableLocalDeclarationStatement, addSecondsArgumentExpression);
            var addSecondsArgumentValueExpression = ArgumentValueExpression(addSecondsArgumentType, addSecondsArgumentExpression, addSecondsArgumentVariable);
            var addMethodReturnType = ReturnedAs(addSecondsInvocationExpression, addMethodReturnedExpression, addMethodParameter);
            var addSecondsArgumentValueType = addSecondsArgumentValueExpression.GigasecondValueType();
            
            return new GigasecondSolution(solution, addMethod, addMethodReturnType, addSecondsInvocationExpression, addSecondsArgumentVariableLocalDeclarationStatement, addSecondsArgumentVariableFieldDeclaration, addSecondsArgumentType, addSecondsArgumentValueExpression, addSecondsArgumentValueType);
        }
        
        private static ClassDeclarationSyntax GigasecondClass(this ParsedSolution solution) =>
            solution.SyntaxRoot.GetClass("Gigasecond");
        
        private static MethodDeclarationSyntax AddMethod(this ClassDeclarationSyntax gigasecondClass) =>
            gigasecondClass?.GetMethod("Add");

        private static InvocationExpressionSyntax AddSecondsInvocationExpression(this MethodDeclarationSyntax addMethod, ParameterSyntax addMethodParameter) =>
            addMethod.DescendantNodes<InvocationExpressionSyntax>().FirstOrDefault(
                invocationExpression =>
                    invocationExpression.Expression.IsEquivalentWhenNormalized(
                        SimpleMemberAccessExpression(
                            IdentifierName(addMethodParameter),
                            IdentifierName("AddSeconds"))));

        private static GigasecondValueType GigasecondValueType(this ExpressionSyntax valueExpression)
        {
            if (valueExpression.IsEquivalentWhenNormalized(GigasecondAsScientificNotation()))
                return Gigasecond.GigasecondValueType.ScientificNotation;
            
            if (valueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithSeparator()))
                return Gigasecond.GigasecondValueType.DigitsWithSeparator;
            
            if (valueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithoutSeparator()))
                return Gigasecond.GigasecondValueType.DigitsWithoutSeparator;
            
            if (valueExpression.IsEquivalentWhenNormalized(GigasecondAsMathPowInvocationExpression()))
                return Gigasecond.GigasecondValueType.MathPow;

            return Gigasecond.GigasecondValueType.Unknown;
        }
    }
}