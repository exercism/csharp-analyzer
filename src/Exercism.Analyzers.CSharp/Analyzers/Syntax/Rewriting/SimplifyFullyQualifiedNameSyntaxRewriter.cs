using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class SimplifyFullyQualifiedNameSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Expression.IsSafeEquivalentTo(SyntaxFactory.IdentifierName("System")))
                return node.Name;

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {
            if (node.Left.IsSafeEquivalentTo(SyntaxFactory.IdentifierName("System")))
                return node.Right;

            return base.VisitQualifiedName(node);
        }
    }
}