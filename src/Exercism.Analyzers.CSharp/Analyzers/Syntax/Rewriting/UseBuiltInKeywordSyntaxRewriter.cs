using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class UseBuiltInKeywordSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {   
            if (node.Expression.IsSafeEquivalentTo(SyntaxFactory.IdentifierName("String")))
                return node.WithExpression(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)));

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {   
            if (node.Left.IsSafeEquivalentTo(SyntaxFactory.IdentifierName("String")))
                return node.WithLeft(SyntaxFactory.IdentifierName("string"));

            return base.VisitQualifiedName(node);
        }
    }
}