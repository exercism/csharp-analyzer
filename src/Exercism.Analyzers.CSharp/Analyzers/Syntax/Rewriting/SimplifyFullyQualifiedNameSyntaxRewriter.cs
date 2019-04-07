using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    internal class SimplifyFullyQualifiedNameSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Expression.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName("System")))
                return base.Visit(node.Name);

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            if (node.Left.IsEquivalentWhenNormalized(SyntaxFactory.IdentifierName("System")))
                return base.Visit(node.Right);

            return base.VisitQualifiedName(node);
        }
    }
}