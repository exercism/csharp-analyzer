using System.Linq;

using Exercism.Analyzers.CSharp.Syntax;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.WeighingMachine
{
    public static class WeighingMachineSyntaxFactory
    {
        public static MemberAccessExpressionSyntax GetMethodCalled(this SyntaxNode syntaxNode, string methodName) =>
            syntaxNode
                .DescendantNodes<InvocationExpressionSyntax>()
                .Select(s => s.Expression)
                .OfType<MemberAccessExpressionSyntax>()
                .FirstOrDefault(s => s.Name.Identifier.Text == methodName);
    }
}
