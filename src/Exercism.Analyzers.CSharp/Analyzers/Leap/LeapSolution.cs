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

        private readonly ClassDeclarationSyntax _leapClass;
        private readonly ParameterSyntax _yearParameter;
        private readonly MethodDeclarationSyntax _isLeapYearMethod;
        private readonly SyntaxNode _returnExpression;

        public LeapSolution(Solution solution) : base(solution)
        {
            _leapClass = LeapClass();
            _isLeapYearMethod = IsLeapYearMethod();
            _yearParameter = YearParameter();
            _returnExpression = ReturnedExpression();
        }

        private ClassDeclarationSyntax LeapClass() =>
            SyntaxRoot.GetClass("Leap");

        private MethodDeclarationSyntax IsLeapYearMethod() =>
            _leapClass?.GetMethod("IsLeapYear");

        private ParameterSyntax YearParameter() =>
            _isLeapYearMethod?.ParameterList.Parameters.FirstOrDefault();

        private ExpressionSyntax ReturnedExpression() =>
            _isLeapYearMethod.ReturnedExpression();

        public string IsLeapYearMethodName =>
            _isLeapYearMethod.Identifier.Text;

        public string YearParameterName =>
            _yearParameter.Identifier.Text;

        public bool UsesDateTimeIsLeapYear() =>
            _isLeapYearMethod.InvokesMethod(IsLeapYearMemberAccessExpression());

        public bool UsesSingleLine() =>
            _isLeapYearMethod.SingleLine();

        public bool UsesExpressionBody() =>
            _isLeapYearMethod.IsExpressionBody();

        public bool UsesTooManyChecks() =>
            _isLeapYearMethod
                .DescendantNodes()
                .OfType<BinaryExpressionSyntax>()
                .Count(BinaryExpressionUsesYearParameter) > MinimalNumberOfChecks;

        public bool UsesIfStatement() =>
            _isLeapYearMethod.UsesIfStatement();

        public bool UsesNestedIfStatement() =>
            _isLeapYearMethod.UsesNestedIfStatement();

        public bool ReturnsMinimumNumberOfChecksInSingleExpression() =>
            Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpression(this)) ||
            Returns(LeapMinimumNumberOfChecksWithoutParenthesesBinaryExpressionReversed(this)) ||
            Returns(LeapMinimumNumberOfChecksWithParenthesesBinaryExpression(this));

        private bool BinaryExpressionUsesYearParameter(BinaryExpressionSyntax binaryExpression) =>
            binaryExpression.Left.IsEquivalentWhenNormalized(
                LeapParameterIdentifierName(this)) ||
            binaryExpression.Right.IsEquivalentWhenNormalized(
                LeapParameterIdentifierName(this));

        private bool Returns(SyntaxNode returned) =>
            _returnExpression.IsEquivalentWhenNormalized(returned);
    }
}