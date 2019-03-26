using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class UnnecessaryParenthesesSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            if (node.Parent is InterpolationSyntax)
                return node.Expression;
            
            return base.VisitParenthesizedExpression(node);
        }
    }
}