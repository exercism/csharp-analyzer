using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax
{
    public static class PropertyDeclarationSyntaxExtensions
    {
        public const string GetAccessorName = "get";
        public const string SetAccessorName = "set";

        public static bool IsAutoProperty(this PropertyDeclarationSyntax property)
        {
            if (property.AccessorList != null)
            {
                return property.AccessorList.Accessors.All(a => a.Body is null && a.ExpressionBody is null);
            }

            return property.ExpressionBody is null;
        }

        public static bool HasInitializer(this PropertyDeclarationSyntax property) => property?.Initializer is not null;

        public static AccessorDeclarationSyntax GetAccessor(this PropertyDeclarationSyntax property, SyntaxKind accessor)
        {
            return property?.AccessorList?.Accessors.FirstOrDefault(ac => ac.IsKind(accessor));
        }

        public static AccessorDeclarationSyntax GetGetAccessor(this PropertyDeclarationSyntax property) =>
            property?.GetAccessor(SyntaxKind.GetAccessorDeclaration);

        public static AccessorDeclarationSyntax GetSetAccessor(this PropertyDeclarationSyntax property) =>
            property?.GetAccessor(SyntaxKind.SetAccessorDeclaration);

        public static string GetBakingFieldName(this PropertyDeclarationSyntax property)
        {
            var get = property.GetGetAccessor();
            var returns = get?.Body.DescendantNodes<ReturnStatementSyntax>().FirstOrDefault();
            var fieldIdentifier = returns?.Expression as IdentifierNameSyntax;
            if (fieldIdentifier is not null)
            { 
                return fieldIdentifier.Identifier.ValueText; 
            }

            var set = property.GetSetAccessor();
            var setValue = set?.Body
                .DescendantNodes<AssignmentExpressionSyntax>()
                .FirstOrDefault(s => s.OperatorToken.IsKind(SyntaxKind.EqualsToken)
                    && s.Right is IdentifierNameSyntax ident && ident.Identifier.ValueText == "value");

            if (setValue is not null && setValue.Left is IdentifierNameSyntax ident)
            {
                return ident.Identifier.ValueText;
            }

            return null;
        }
    }
}
