using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    internal class AddBracesSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitIfStatement(IfStatementSyntax ifStatement)
        {
            if (ifStatement.Statement is BlockSyntax)
                return base.VisitIfStatement(ifStatement);

            return base.VisitIfStatement(
                ifStatement.WithStatement(
                    Block(
                        SingletonList(
                            ifStatement.Statement))));
        }
    }
}