using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    internal class RemoveUnneededParenthesesRule : Rule
    {
        public override Task<Diagnostic[]> Verify(CompiledSolution compiledSolution)
        {
            //        https://stackoverflow.com/questions/43314098/roslyn-how-to-determine-if-an-expression-has-operators-with-equal-lower-precede
//        public static ExpressionSyntax AddParentheses(this ExpressionSyntax expression)
//        {
//            switch (expression.RawKind)
//            {
//                case (int)SyntaxKind.ParenthesizedExpression:
//                case (int)SyntaxKind.IdentifierName:
//                case (int)SyntaxKind.QualifiedName:
//                case (int)SyntaxKind.SimpleMemberAccessExpression:
//                case (int)SyntaxKind.InterpolatedStringExpression:
//                case (int)SyntaxKind.NumericLiteralExpression:
//                case (int)SyntaxKind.StringLiteralExpression:
//                case (int)SyntaxKind.CharacterLiteralExpression:
//                case (int)SyntaxKind.TrueLiteralExpression:
//                case (int)SyntaxKind.FalseLiteralExpression:
//                case (int)SyntaxKind.NullLiteralExpression:
//                    return expression;
//
//                default:
//                    return SyntaxFactory
//                        .ParenthesizedExpression(expression)
//                        .WithAdditionalAnnotations(Simplifier.Annotation);
//            }
//        }
            
            throw new System.NotImplementedException();
        }
    }
}