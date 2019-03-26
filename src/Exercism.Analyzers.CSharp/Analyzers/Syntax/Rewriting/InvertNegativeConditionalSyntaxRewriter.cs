using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class InvertNegativeConditionalSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            if (node.Condition is BinaryExpressionSyntax binaryExpression &&
                binaryExpression.IsKind(SyntaxKind.NotEqualsExpression))
                return InvertConditionalExpression(node, InvertBinaryExpression(binaryExpression));

            if (node.Condition is PrefixUnaryExpressionSyntax prefixUnaryExpression &&
                prefixUnaryExpression.IsKind(SyntaxKind.LogicalNotExpression))
                return InvertConditionalExpression(node, InvertPrefixUnaryExpression(prefixUnaryExpression));

            return base.VisitConditionalExpression(node);
        }

        private static ConditionalExpressionSyntax InvertConditionalExpression(ConditionalExpressionSyntax node, ExpressionSyntax condition) =>
            node
                .WithCondition(condition)
                .WithWhenTrue(node.WhenFalse)
                .WithWhenFalse(node.WhenTrue);

        private static BinaryExpressionSyntax InvertBinaryExpression(BinaryExpressionSyntax binaryExpression) =>
            SyntaxFactory.BinaryExpression(
                SyntaxKind.EqualsExpression,
                binaryExpression.Left,
                binaryExpression.Right);

        private static ExpressionSyntax InvertPrefixUnaryExpression(PrefixUnaryExpressionSyntax prefixUnaryExpression) =>
            prefixUnaryExpression.Operand;
    }
}