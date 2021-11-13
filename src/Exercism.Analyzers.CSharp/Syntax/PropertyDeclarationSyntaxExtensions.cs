using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return property.AccessorList.Accessors.All(a => a.Body is null || a.ExpressionBody is null);
            }

            return property.ExpressionBody is null;
        }

        public static bool HasInitializer(this PropertyDeclarationSyntax property) => property?.Initializer is not null;

        public static AccessorDeclarationSyntax GetAccessor(this PropertyDeclarationSyntax property, string accessor)
        {
            return property?.AccessorList?.Accessors.Where(ac => ac.Keyword.Text == accessor).FirstOrDefault();
        }

        public static AccessorDeclarationSyntax GetGetAccessor(this PropertyDeclarationSyntax property) => 
            property?.GetAccessor(GetAccessorName);

        public static AccessorDeclarationSyntax GetSetAccessor(this PropertyDeclarationSyntax property) => 
            property?.GetAccessor(SetAccessorName);
    }
}
