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
                return base.VisitConditionalExpression(InvertConditionalExpression(node, InvertBinaryExpression(binaryExpression)));

            if (node.Condition is PrefixUnaryExpressionSyntax prefixUnaryExpression &&
                prefixUnaryExpression.IsKind(SyntaxKind.LogicalNotExpression))
                return base.VisitConditionalExpression(InvertConditionalExpression(node, InvertPrefixUnaryExpression(prefixUnaryExpression)));

            return base.VisitConditionalExpression(node);
        }

        private static ConditionalExpressionSyntax InvertConditionalExpression(ConditionalExpressionSyntax node, ExpressionSyntax condition) =>
            node
                .WithCondition(condition.WithTriviaFrom(node.Condition))
                .WithWhenTrue(node.WhenFalse.WithTriviaFrom(node.WhenTrue))
                .WithWhenFalse(node.WhenTrue.WithTriviaFrom(node.WhenFalse));

        private static BinaryExpressionSyntax InvertBinaryExpression(BinaryExpressionSyntax binaryExpression) =>
            SyntaxFactory.BinaryExpression(
                SyntaxKind.EqualsExpression,
                binaryExpression.Left,
                binaryExpression.Right);

        private static ExpressionSyntax InvertPrefixUnaryExpression(PrefixUnaryExpressionSyntax prefixUnaryExpression) =>
            prefixUnaryExpression.Operand;
    }
}