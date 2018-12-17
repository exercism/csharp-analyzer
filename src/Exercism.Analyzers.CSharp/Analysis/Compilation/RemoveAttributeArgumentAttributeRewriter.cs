using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analysis.Compilation
{
    internal class RemoveAttributeArgumentAttributeRewriter : CSharpSyntaxRewriter
    {
        private readonly string _attributeArgumentName;
        private readonly string _attributeName;
     
        public RemoveAttributeArgumentAttributeRewriter(string attributeArgumentName, string attributeName)
            => (_attributeArgumentName, _attributeName) = (attributeArgumentName, attributeName);
                 
        public override SyntaxNode VisitAttributeArgument(AttributeArgumentSyntax node) 
            => AttributeArgumentNameMatches(node) && AttributeNameMatches(node) ? null : base.VisitAttributeArgument(node);
     
        private bool AttributeArgumentNameMatches(AttributeArgumentSyntax node) 
            => node.NameEquals.Name.GetName() == _attributeArgumentName;
     
        private bool AttributeNameMatches(SyntaxNode node)
            => node.Parent.Parent is AttributeSyntax attributeSyntax && attributeSyntax.Name.GetName() == _attributeName;
    }
}