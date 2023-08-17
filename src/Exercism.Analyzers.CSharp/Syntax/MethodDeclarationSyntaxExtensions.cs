using System.Linq;

using Exercism.Analyzers.CSharp.Analyzers.Shared;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax;

internal static class MethodDeclarationSyntaxExtensions
{
    public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, ParameterSyntax parameter) =>
        methodDeclaration != null &&
        methodDeclaration.AssignsToIdentifier(SharedSyntaxFactory.IdentifierName(parameter));

    public static bool CanUseExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
        methodDeclaration?.ExpressionBody == null &&
        methodDeclaration?.Body.Statements is [ReturnStatementSyntax];

    public static ExpressionSyntax ReturnedExpression(this MethodDeclarationSyntax methodDeclaration) =>
        methodDeclaration?.ExpressionBody?.Expression ??
        methodDeclaration?.Body
            .DescendantNodes<ReturnStatementSyntax>()
            .Select(returnStatement => returnStatement.Expression)
            .LastOrDefault();

    public static bool IsExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
        methodDeclaration?.ExpressionBody != null;

    public static ParameterSyntax FirstParameter(this MethodDeclarationSyntax methodDeclaration) =>
        methodDeclaration?.ParameterList.Parameters[0];

    public static bool UsesIfStatement(this MethodDeclarationSyntax methodDeclaration) =>
        methodDeclaration != null &&
        methodDeclaration
            .DescendantNodes()
            .OfType<IfStatementSyntax>()
            .Any();

    public static bool UsesNestedIfStatement(this MethodDeclarationSyntax methodDeclaration) =>
        methodDeclaration != null &&
        methodDeclaration
            .DescendantNodes()
            .OfType<IfStatementSyntax>()
            .Any(ifStatement => ifStatement.Statement.DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any());
}