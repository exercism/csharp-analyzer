using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analysis.Testing
{
    internal class RemoveSkipAttributeArgumentSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitAttributeArgument(AttributeArgumentSyntax node) 
            => AttributeArgumentNameMatches(node) ? null : base.VisitAttributeArgument(node);
     
        private static bool AttributeArgumentNameMatches(AttributeArgumentSyntax node) 
            => node.NameEquals?.Name.GetName() == "Skip";
    }
}