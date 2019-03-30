using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class UseBuiltInKeywordSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {   
            if (node.Expression.IsSafeEquivalentTo(IdentifierName("String")))
                return node.WithExpression(
                    PredefinedType(
                        Token(SyntaxKind.StringKeyword)));

            return base.VisitMemberAccessExpression(node);
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if (node.Type.IsSafeEquivalentTo(IdentifierName("String")))
                return node.WithType(
                    PredefinedType(
                        Token(SyntaxKind.StringKeyword))
                    .WithTriviaFrom(node.Type));
            
            return base.VisitVariableDeclaration(node);
        }

        public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node)
        {   
            if (node.Left.IsSafeEquivalentTo(IdentifierName("String")))
                return node.WithLeft(
                    IdentifierName("string").WithTriviaFrom(node.Left));

            return base.VisitQualifiedName(node);
        }
    }
}