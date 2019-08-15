using Exercism.Analyzers.CSharp.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax.Rewriting
{
    internal class UseBuiltInKeywordSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Expression.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName("String")))
                return base.Visit(
                    node.WithExpression(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.StringKeyword))));

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if (node.Type.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName("String")))
                return base.Visit(
                    node.WithType(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(SyntaxKind.StringKeyword))
                        .WithTriviaFrom(node.Type)));

            return base.VisitVariableDeclaration(node);
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            if (node.Left.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName("String")))
                return base.Visit(
                    node.WithLeft(
                        SyntaxFactory.IdentifierName("string").WithTriviaFrom(node.Left)));

            return base.VisitQualifiedName(node);
        }
    }
}