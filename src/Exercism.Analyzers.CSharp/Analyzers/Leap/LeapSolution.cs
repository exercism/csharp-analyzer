using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal class LeapSolution : Solution
    {
        private const int MinimalNumberOfChecks = 3;

        private readonly ParameterSyntax _yearParameter;
        private readonly MethodDeclarationSyntax _isLeapYearMethod;
        private readonly SyntaxNode _returnExpression;

        public LeapSolution(Solution solution,
            MethodDeclarationSyntax isLeapYearMethod,
            ParameterSyntax yearParameter,
            SyntaxNode returnExpression) : base(solution.Slug, solution.Name, solution.SyntaxRoot)
        {
            _yearParameter = yearParameter;
            _isLeapYearMethod = isLeapYearMethod;
            _returnExpression = returnExpression;
        }

        public string IsLeapYearMethodName =>
            _isLeapYearMethod.Identifier.Text;

        public string YearParameterName =>
            _yearParameter.Identifier.Text;

        public bool UsesDateTimeIsLeapYear() =>
            _isLeapYearMethod.InvokesMethod(
                DateTimeMemberAccessExpression(SyntaxFactory.IdentifierName("IsLeapYear")));

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

        private bool Returns(SyntaxNode returned) => _returnExpression.IsEquivalentWhenNormalized(returned);
    }
}