using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal class TwoFerSolution : Solution
    {
        private readonly TwoFerError _twoFerError;
        private readonly MethodDeclarationSyntax _speakMethod;
        private readonly ExpressionSyntax _twoFerExpression;
        private readonly ParameterSyntax _speakMethodParameter;
        private readonly VariableDeclaratorSyntax _twoFerVariable;

        public TwoFerSolution(
            Solution solution,
            MethodDeclarationSyntax speakMethod,
            ParameterSyntax speakMethodParameter,
            ExpressionSyntax twoFerExpression,
            VariableDeclaratorSyntax twoFerVariableDeclarator,
            TwoFerError twoFerError) : base(solution.Slug, solution.Name, solution.SyntaxRoot)
        {
            _twoFerError = twoFerError;
            _speakMethod = speakMethod;
            _speakMethodParameter = speakMethodParameter;
            _twoFerExpression = twoFerExpression;
            _twoFerVariable = twoFerVariableDeclarator;
        }

        public string SpeakMethodName =>
            _speakMethod.Identifier.Text;

        public string SpeakMethodParameterName =>
            _speakMethodParameter.Identifier.Text;

        public string SpeakMethodParameterDefaultValue =>
            _speakMethodParameter.Default.Value.ToFullString();

        public string TwoFerVariableName =>
            _twoFerVariable.Identifier.Text;

        public bool MissingSpeakMethod =>
            _twoFerError == TwoFerError.MissingSpeakMethod;

        public bool InvalidSpeakMethod =>
            _twoFerError == TwoFerError.InvalidSpeakMethod;

        public bool UsesOverloads =>
            _twoFerError == TwoFerError.UsesOverloads;

        public bool UsesDuplicateString =>
            _twoFerError == TwoFerError.UsesDuplicateString;

        public bool UsesStringJoin =>
            _twoFerError == TwoFerError.UsesStringJoin;

        public bool UsesStringConcat =>
            _twoFerError == TwoFerError.UsesStringConcat;

        public bool UsesStringReplace =>
            _twoFerError == TwoFerError.UsesStringReplace;

        public bool NoDefaultValue =>
            _twoFerError == TwoFerError.NoDefaultValue;

        public bool InvalidDefaultValue =>
            _twoFerError == TwoFerError.InvalidDefaultValue;

        public bool AssignsToParameter() =>
            _speakMethod.AssignsToParameter(_speakMethodParameter);

        public bool UsesSingleLine() =>
            _speakMethod.SingleLine();

        public bool UsesExpressionBody() =>
            _speakMethod.IsExpressionBody();

        public bool ReturnsStringConcatenation() =>
            ReturnsStringConcatenationWithDefaultValue() ||
            ReturnsStringConcatenationWithNullCoalescingOperator() ||
            ReturnsStringConcatenationWithTernaryOperator();

        private bool ReturnsStringConcatenationWithDefaultValue() =>
            Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerParameterIdentifierName(this)));

        private bool ReturnsStringConcatenationWithNullCoalescingOperator() =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            TwoFerParameterIdentifierName(this)))));

        private bool ReturnsStringConcatenationWithTernaryOperator() =>
            ReturnsStringConcatenationWithNullCheck() ||
            ReturnsStringConcatenationWithIsNullOrEmptyCheck() ||
            ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck();

        private bool ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck() =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                            TwoFerParameterIdentifierName(this)))));

        private bool ReturnsStringConcatenationWithIsNullOrEmptyCheck() =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(this),
                            TwoFerParameterIdentifierName(this)))));

        private bool ReturnsStringConcatenationWithNullCheck() =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpressionWithNullCheck(this))));

        public bool ReturnsStringFormat() =>
            ReturnsStringFormatWithDefaultValue() ||
            ReturnsStringFormatWithNullCoalescingOperator() ||
            ReturnsStringFormatWithTernaryOperator();

        private bool ReturnsStringFormatWithDefaultValue() =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerParameterIdentifierName(this)));

        private bool ReturnsStringFormatWithNullCoalescingOperator() =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(this))));

        private bool ReturnsStringFormatWithTernaryOperator() =>
            ReturnsStringFormatWithNullCheck() ||
            ReturnsStringFormatWithIsNullOrEmptyCheck() ||
            ReturnsStringFormatWithIsNullOrWhiteSpaceCheck();

        private bool ReturnsStringFormatWithIsNullOrWhiteSpaceCheck() =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                        TwoFerParameterIdentifierName(this))));

        private bool ReturnsStringFormatWithIsNullOrEmptyCheck() =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(this),
                        TwoFerParameterIdentifierName(this))));

        private bool ReturnsStringFormatWithNullCheck() =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpressionWithNullCheck(this)));

        public bool ReturnsStringInterpolation() =>
            ReturnsStringInterpolationWithDefaultValue() ||
            ReturnsStringInterpolationWithNullCheck() ||
            ReturnsStringInterpolationWithNullCoalescingOperator() ||
            ReturnsStringInterpolationWithIsNullOrEmptyCheck() ||
            ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck();

        public bool ReturnsStringInterpolationWithDefaultValue() =>
            Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerParameterIdentifierName(this)));

        public bool ReturnsStringInterpolationWithNullCheck() =>
            Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    EqualsExpression(
                        TwoFerParameterIdentifierName(this),
                        NullLiteralExpression()),
                    TwoFerParameterIdentifierName(this)));

        public bool ReturnsStringInterpolationWithNullCoalescingOperator() =>
            Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(this))));

        public bool ReturnsStringInterpolationWithIsNullOrEmptyCheck() =>
            Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    TwoFerIsNullOrEmptyInvocationExpression(this),
                    TwoFerParameterIdentifierName(this)));

        public bool ReturnsStringInterpolationWithIsNullOrWhiteSpaceCheck() =>
            Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                    TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingKnownExpression() =>
            AssignsParameterUsingNullCoalescingOperator() ||
            AssignsParameterUsingNullCheck() ||
            AssignsParameterUsingIfNullCheck() ||
            AssignsParameterUsingIsNullOrEmptyCheck() ||
            AssignsParameterUsingIfIsNullOrEmptyCheck() ||
            AssignsParameterUsingIsNullOrWhiteSpaceCheck() ||
            AssignsParameterUsingIfIsNullOrWhiteSpaceCheck();

        public bool AssignsParameterUsingNullCoalescingOperator() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                TwoFerCoalesceExpression(
                    TwoFerParameterIdentifierName(this)),
                TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingNullCheck() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                TwoFerParameterIsNullConditionalExpression(this),
                TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingIfNullCheck() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                TwoFerParameterIsNullExpression(this),
                TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingIsNullOrEmptyCheck() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                TwoFerParameterIsNullOrEmptyConditionalExpression(this),
                TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingIfIsNullOrEmptyCheck() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                TwoFerIsNullOrEmptyInvocationExpression(this),
                TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingIsNullOrWhiteSpaceCheck() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(this),
                TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingIfIsNullOrWhiteSpaceCheck() =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                TwoFerParameterIdentifierName(this)));

        private bool ParameterAssignedUsingStatement(SyntaxNode statement) =>
            AssignmentStatement().IsEquivalentWhenNormalized(statement);

        private StatementSyntax AssignmentStatement() =>
            _speakMethod.Body.Statements[0];

        public bool AssignsVariable() =>
            _twoFerVariable != null;

        public bool AssignsVariableUsingKnownInitializer() =>
            AssignsVariableUsingNullCoalescingOperator() ||
            AssignsVariableUsingNullCheck() ||
            AssignsVariableUsingIsNullOrEmptyCheck() ||
            AssignsVariableUsingIsNullOrWhiteSpaceCheck();

        public bool AssignsVariableUsingNullCoalescingOperator() =>
            AssignsVariableUsingExpression(
                TwoFerCoalesceExpression(
                    TwoFerParameterIdentifierName(this)));

        public bool AssignsVariableUsingNullCheck() =>
            AssignsVariableUsingExpression(TwoFerParameterIsNullConditionalExpression(this));

        public bool AssignsVariableUsingIsNullOrEmptyCheck() =>
            AssignsVariableUsingExpression(TwoFerParameterIsNullOrEmptyConditionalExpression(this));

        public bool AssignsVariableUsingIsNullOrWhiteSpaceCheck() =>
            AssignsVariableUsingExpression(TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(this));

        private bool AssignsVariableUsingExpression(ExpressionSyntax initializer) =>
            _twoFerVariable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(initializer));

        public bool ReturnsStringInterpolationWithVariable() =>
            Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerVariableIdentifierName(this)));

        public bool ReturnsStringConcatenationWithVariable() =>
            Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerVariableIdentifierName(this)));

        public bool ReturnsStringFormatWithVariable() =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerVariableIdentifierName(this)));

        private bool Returns(SyntaxNode returned) => _twoFerExpression.IsEquivalentWhenNormalized(returned);
    }
}