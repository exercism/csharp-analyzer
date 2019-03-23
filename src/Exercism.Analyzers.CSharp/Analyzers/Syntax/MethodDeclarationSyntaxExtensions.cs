using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class MethodDeclarationSyntaxExtensions
    {
        public static bool HasParameter(this MethodDeclarationSyntax methodDeclaration, string parameterName) =>
            methodDeclaration?
                .ParameterList
                .Parameters
                .Any(parameter => parameter.Identifier.ValueText == parameterName) ?? false;

        public static bool AssignsToParameter(this MethodDeclarationSyntax methodDeclaration, string parameterName) =>
            methodDeclaration.HasParameter(parameterName) &&
            methodDeclaration.AssignsToIdentifier(parameterName);

        public static bool IsBlockBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.Body != null;
        
        public static bool IsExpressionBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.ExpressionBody != null;

        public static bool CanBeConvertedToBlockBody(this MethodDeclarationSyntax methodDeclaration) =>
            methodDeclaration.Body.Statements.Count <= 1;
    }
}