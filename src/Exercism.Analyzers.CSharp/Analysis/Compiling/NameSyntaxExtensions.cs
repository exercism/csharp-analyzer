using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analysis.Compiling
{
    internal static class NameSyntaxExtensions
    {
        public static string GetName(this NameSyntax nameSyntax)
        {
            switch (nameSyntax)
            {
                case AliasQualifiedNameSyntax aliasQualifiedNameSyntax:
                    return aliasQualifiedNameSyntax.Name.GetName();
                case GenericNameSyntax genericNameSyntax:
                    return genericNameSyntax.Identifier.Text;
                case IdentifierNameSyntax identifierNameSyntax:
                    return identifierNameSyntax.Identifier.Text;
                case QualifiedNameSyntax qualifiedNameSyntax:
                    return $"{qualifiedNameSyntax.Left.GetName()}.{qualifiedNameSyntax.Right.GetName()}";
                case SimpleNameSyntax simpleNameSyntax:
                    return simpleNameSyntax.Identifier.Text;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nameSyntax));
            }
        }
    }
}