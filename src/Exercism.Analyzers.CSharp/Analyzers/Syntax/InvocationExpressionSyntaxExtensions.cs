using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class InvocationExpressionSyntaxExtensions
    {
        public static ExpressionSyntax FirstArgumentExpression(this InvocationExpressionSyntax invocationExpression) =>
            invocationExpression?.ArgumentList.Arguments[0].Expression;
    }
}