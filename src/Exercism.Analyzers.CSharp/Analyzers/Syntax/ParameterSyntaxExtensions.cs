using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class ParameterSyntaxExtensions
    {
        public static MemberAccessExpressionSyntax ToMemberAccessExpression(this ParameterSyntax parameter, string member) =>
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(parameter.Identifier),
                IdentifierName(member));
    }
}