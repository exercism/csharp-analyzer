using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class MethodDeclarationSyntaxExtensions
    {
        public static bool InvokesExpression(this MethodDeclarationSyntax methodDeclaration, InvocationExpressionSyntax invocationExpression) =>
            methodDeclaration?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(descendantInvocationExpression => descendantInvocationExpression.IsEquivalentWhenNormalized(invocationExpression)) ?? false;
        
        public static bool InvokesExpression(this MethodDeclarationSyntax methodDeclaration, ExpressionSyntax expression) =>
            methodDeclaration?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression => invocationExpression.Expression.IsEquivalentWhenNormalized(expression)) ?? false;

        public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, ParameterSyntax parameter) =>
            methodDeclaration.AssignsToIdentifier(SyntaxFactory.IdentifierName(parameter.Identifier));

        public static bool SingleLine(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null ||
            methodDeclaration.Body.Statements.Count == 1;

        public static ExpressionSyntax ReturnedExpression(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody?.Expression ??
            methodDeclaration.Body
                .DescendantNodes<ReturnStatementSyntax>()
                .Select(returnStatement => returnStatement.Expression)
                .LastOrDefault();

        public static bool IsBlockBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.Body != null;
        
        public static bool IsExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null;

        public static bool CanConvertToExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.Body != null &&
            methodDeclaration.Body.Statements.Count == 1;

        public static string FirstParameterName(this MethodDeclarationSyntax methodDeclarationSyntax) =>
            methodDeclarationSyntax.ParameterList.Parameters[0].Identifier.ValueText;

        public static string FirstParameterName(this ExpressionSyntax expression) =>
            expression.ParentMethod().FirstParameterName();
    }
}