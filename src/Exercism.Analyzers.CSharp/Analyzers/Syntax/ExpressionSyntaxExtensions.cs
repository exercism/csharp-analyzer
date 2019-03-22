using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax
{
    internal static class ExpressionSyntaxExtensions
    {
        public static bool CreatesObjectOfType(this ExpressionSyntax expression, string type) =>
            expression is ObjectCreationExpressionSyntax objectCreationExpression &&
            objectCreationExpression.Type.ToFullString() == type;
    }
}