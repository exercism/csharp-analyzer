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
    }
}