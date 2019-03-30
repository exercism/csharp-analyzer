using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting
{
    public class AddBracesSyntaxRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitIfStatement(IfStatementSyntax ifStatement)
        {
            if (ifStatement.Statement is BlockSyntax)
                return ifStatement;

            return ifStatement.WithStatement(
                    SyntaxFactory.Block(
                        SyntaxFactory.SingletonList(
                            ifStatement.Statement)));
        }
    }
}