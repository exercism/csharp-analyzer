using System.Linq;

using Exercism.Analyzers.CSharp.Syntax;
using Exercism.Analyzers.CSharp.Syntax.Comparison;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Exercism.Analyzers.CSharp.Analyzers.Leap.LeapSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal class LeapSolution : Solution
    {
        public const string LeapClassName = "Leap";
        public const string IsLeapYearMethodName = "IsLeapYear";
        public const string IsLeapYearMethodSignature = "public static bool IsLeapYear(int year)";

        private const int MinimalNumberOfChecks = 3;

        public LeapSolution(Solution solution) : base(solution)
        {
        }

        private ClassDeclarationSyntax LeapClass =>
            SyntaxRoot.GetClass(LeapClassName);

        private MethodDeclarationSyntax IsLeapYearMethod =>
            LeapClass?.GetMethod(IsLeapYearMethodName);

        public bool MissingLeapClass =>
            LeapClass == null;

        public bool MissingIsLeapYearMethod =>
            !MissingLeapClass && IsLeapYearMethod == null;

        // TODO: create helper method to verify type signature
        // TODO: create helper method for predefined types
        public bool InvalidIsLeapYearMethod =>
            IsLeapYearMethod != null &&
            (IsLeapYearMethod.ParameterList.Parameters.Count != 1 ||
            !IsLeapYearMethod.ParameterList.Parameters[0].Type.IsEquivalentWhenNormalized(
                PredefinedType(
                    Token(SyntaxKind.IntKeyword))) ||
            !IsLeapYearMethod.ReturnType.IsEquivalentWhenNormalized(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword))));

        private ParameterSyntax YearParameter =>
            IsLeapYearMethod?.ParameterList.Parameters.FirstOrDefault();

        private ExpressionSyntax ReturnedExpression =>
            IsLeapYearMethod?.ReturnedExpression();

        public string YearParameterName =>
            YearParameter?.Identifier.Text;

        public bool UsesDateTimeIsLeapYear =>
            IsLeapYearMethod.InvokesMethod(IsLeapYearMemberAccessExpression());

        public bool UsesSingleLine =>
            IsLeapYearMethod.SingleLine();

        public bool UsesExpressionBody =>
            IsLeapYearMethod.IsExpressionBody();

        public bool UsesTooManyChecks =>
            IsLeapYearMethod
                .DescendantNodes<BinaryExpressionSyntax>()
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