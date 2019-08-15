using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax
{
    internal static class MethodDeclarationSyntaxExtensions
    {
        public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, ParameterSyntax parameter) =>
            methodDeclaration.AssignsToIdentifier(SharedSyntaxFactory.IdentifierName(parameter));

        public static bool SingleLine(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null ||
            methodDeclaration.Body.Statements.Count == 1;

        public static ExpressionSyntax ReturnedExpression(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody?.Expression ??
            methodDeclaration.Body
                .DescendantNodes<ReturnStatementSyntax>()
                .Select(returnStatement => returnStatement.Expression)
                .LastOrDefault();

        public static bool IsExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null;

        public static ParameterSyntax FirstParameter(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration?.ParameterList.Parameters[0];

        public static bool UsesIfStatement(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration
                .DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any();

        public static bool UsesNestedIfStatement(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration
                .DescendantNodes()
                .OfType<IfStatementSyntax>()
                .Any(ifStatement => ifStatement.DescendantNodes()
                    .OfType<IfStatementSyntax>()
                    .Any());
    }
}