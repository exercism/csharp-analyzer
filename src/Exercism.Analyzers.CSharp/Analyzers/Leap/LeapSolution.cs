using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal class LeapSolution : Solution
    {
        private const int MinimalNumberOfChecks = 3;

        public LeapSolution(Solution solution) : base(solution)
        {
        }

        private ClassDeclarationSyntax LeapClass =>
            SyntaxRoot.GetClass("Leap");

        private MethodDeclarationSyntax IsLeapYearMethod =>
            LeapClass?.GetMethod("IsLeapYear");

        private ParameterSyntax YearParameter =>
            IsLeapYearMethod?.ParameterList.Parameters.FirstOrDefault();

        private ExpressionSyntax ReturnedExpression =>
            IsLeapYearMethod.ReturnedExpression();

        public string IsLeapYearMethodName =>
            IsLeapYearMethod.Identifier.Text;

        public string YearParameterName =>
            YearParameter.Identifier.Text;

        public bool UsesDateTimeIsLeapYear =>
            IsLeapYearMethod.InvokesMethod(IsLeapYearMemberAccessExpression());

        public bool UsesSingleLine =>
            IsLeapYearMethod.SingleLine();

        public bool UsesExpressionBody =>
            IsLeapYearMethod.IsExpressionBody();

        public bool UsesTooManyChecks =>
            IsLeapYearMethod
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count(BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;

        public bool UsesIfStatement =>
            IsLeapYearMethod.UsesIfStatement();

        public bool UsesNestedIfStatement =>
            IsLeapYearMethod.UsesNestedIfStatement();

        public bool ReturnsMinimumNumberOfChecksInSingleExpression =>
            Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(this)) ||
            Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpressionReversed(this)) ||
            Returns(LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(this));

        private bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
            binaryExpression.Left.IsEquivalentWhenNormalized(
                LeapParameterIdentifierName(this)) ||
            binaryExpression.Right.IsEquivalentWhenNormalized(
                LeapParameterIdentifierName(this));

        private bool Returns(SyntaxNode returned) =>
            ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}