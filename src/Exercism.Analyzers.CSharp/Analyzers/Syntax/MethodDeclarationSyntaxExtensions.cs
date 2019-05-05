using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class MethodDeclarationSyntaxExtensions
    {   
        public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, ParameterSyntax parameter) =>
            methodDeclaration.AssignsToIdentifier(IdentifierName(parameter));

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
    }
}