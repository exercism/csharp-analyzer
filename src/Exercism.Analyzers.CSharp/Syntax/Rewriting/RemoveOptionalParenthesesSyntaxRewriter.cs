using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Syntax.Rewriting
{
    internal class RemoveOptionalParenthesesSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            if (node.Parent is ConditionalExpressionSyntax)
                return base.Visit(node.Expression);

            return base.VisitParenthesizedExpression(node);
        }
    }
}