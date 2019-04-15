using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class MethodDeclarationSyntaxExtensions
    {
        public static bool InvokesExpression(this MethodDeclarationSyntax methodDeclaration, ExpressionSyntax expression) =>
            methodDeclaration?
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression => invocationExpression.Expression.IsEquivalentWhenNormalized(expression)) ?? false;

        public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, ParameterSyntax parameter) =>
            methodDeclaration.AssignsToIdentifier(
                IdentifierName(parameter.Identifier));

        public static bool SingleLine(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null ||
            methodDeclaration.Body.Statements.Count == 1;

        public static ExpressionSyntax ReturnedExpression(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody?.Expression ??
            methodDeclaration.Body
                .DescendantNodes<ReturnStatementSyntax>()
                .Select(returnStatement => returnStatement.Expression)
                .LastOrDefault();

        public static ExpressionSyntax ExpressionDirectlyReturned(this MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.ExpressionBody != null)
                return methodDeclaration.ExpressionBody.Expression;

            if (methodDeclaration.Body.Statements.Count != 1)
                return null;

            var returnStatement = methodDeclaration.Body.Statements[0] as ReturnStatementSyntax;
            return returnStatement?.Expression;
        }

        public static bool IsExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null;

        public static ExpressionSyntax ExpressionAssignedToVariableAndReturned(this MethodDeclarationSyntax nameMethod)
        {
            if (nameMethod?.Body?.Statements.Count != 2)
                return null;

            var localDeclaration = nameMethod.Body.Statements[0] as LocalDeclarationStatementSyntax;
            var returnStatement = nameMethod.Body.Statements[1] as ReturnStatementSyntax;
            
            if (localDeclaration == null || returnStatement == null)
                return null;

            if (localDeclaration.Declaration.Variables.Count != 1 ||
                !returnStatement.ReturnsVariable(localDeclaration.Declaration.Variables[0]))
                return null;

            return localDeclaration.Declaration.Variables[0].Initializer.Value;
        }

        public static ExpressionSyntax ExpressionAssignedToParameterAndReturned(this MethodDeclarationSyntax nameMethod, ParameterSyntax parameter)
        {
            if (nameMethod?.Body?.Statements.Count != 2 || parameter == null)
                return null;

            var expressionStatementSyntax = nameMethod.Body.Statements[0] as ExpressionStatementSyntax;
            var returnStatement = nameMethod.Body.Statements[1] as ReturnStatementSyntax;

            if (expressionStatementSyntax == null || returnStatement == null)
                return null;

            var assignmentExpression = expressionStatementSyntax.Expression as AssignmentExpressionSyntax;
            if (assignmentExpression == null)
                return null;

            if (!assignmentExpression.Left.IsEquivalentWhenNormalized(IdentifierName(parameter.Identifier)) ||
                !returnStatement.ReturnsParameter(parameter))
                return null;

            return assignmentExpression.Right;
        }
    }
}