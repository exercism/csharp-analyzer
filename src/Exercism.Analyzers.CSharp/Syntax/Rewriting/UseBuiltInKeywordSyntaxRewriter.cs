using Exercism.Analyzers.CSharp.Syntax.Comparison;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Syntax.Rewriting
{
    internal class UseBuiltInKeywordSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Expression.IsEquivalentWhenNormalized(IdentifierName("String")))
                return base.Visit(
                    node.WithExpression(
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword))
                        .WithTriviaFrom(node.Expression)));

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if (node.Type.IsEquivalentWhenNormalized(IdentifierName("String")))
                return base.Visit(
                    node.WithType(
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword))
                        .WithTriviaFrom(node.Type)));

            return base.VisitVariableDeclaration(node);
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            if (node.Left.IsEquivalentWhenNormalized(IdentifierName("String")))
                return base.Visit(
                    node.WithLeft(
                        IdentifierName("string").WithTriviaFrom(node.Left)));

            return base.VisitQualifiedName(node);
        }

        public override SyntaxNode VisitParameter(ParameterSyntax node)
        {
            if (node.Type.IsEquivalentWhenNormalized(IdentifierName("String")))
                return base.Visit(
                    node.WithType(
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword))
                            .WithTriviaFrom(node.Type)));

            return base.VisitParameter(node);
        }
    }
}