using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class ExponentNotationSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (node.IsKind(SyntaxKind.NumericLiteralExpression) &&
                node.Token.IsKind(SyntaxKind.NumericLiteralToken) &&
                node.Token.Value is double value &&
                node.Token.Text.Contains("e"))
                return base.VisitLiteralExpression(
                    node.WithToken(
                        SyntaxFactory.Literal(
                            node.Token.Text.Replace("e", "E"),
                            value)));
            
            return base.VisitLiteralExpression(node);
        }
    }
}