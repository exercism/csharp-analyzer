using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal class TwoFerSolution : Solution
    {
        private readonly ClassDeclarationSyntax _twoFerClass;
        private readonly MethodDeclarationSyntax _speakMethod;
        private readonly ExpressionSyntax _twoFerExpression;
        private readonly ParameterSyntax _speakMethodParameter;
        private readonly VariableDeclaratorSyntax _twoFerVariable;

        public TwoFerSolution(Solution solution) : base(solution)
        {
            _twoFerClass = TwoFerClass();
            _speakMethod = SpeakMethod();
            _speakMethodParameter = SpeakMethodParameter();
            _twoFerExpression = TwoFerExpression();
            _twoFerVariable = TwoFerVariable();
        }

        private ClassDeclarationSyntax TwoFerClass() =>
            SyntaxRoot.GetClass("TwoFer");

        private MethodDeclarationSyntax SpeakMethod() =>
            _twoFerClass?.GetMethod("Speak");

        private ParameterSyntax SpeakMethodParameter() =>
            _speakMethod?.ParameterList.Parameters.FirstOrDefault();

        private ExpressionSyntax TwoFerExpression() =>
            _speakMethod?.ReturnedExpression();

        private VariableDeclaratorSyntax TwoFerVariable()
        {
            if (_speakMethod == null ||
                _speakMethod.Body == null ||
                _speakMethod.Body.Statements.Count != 2)
                return null;

            if (!(_speakMethod.Body.Statements[1] is ReturnStatementSyntax) ||
                !(_speakMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration))
                return null;

            if (localDeclaration.Declaration.Variables.Count != 1 ||
                !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
                !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(IdentifierName("var")))
                return null;

            return localDeclaration.Declaration.Variables[0];
        }

        public string SpeakMethodName =>
            _speakMethod.Identifier.Text;

        public string SpeakMethodParameterName =>
            _speakMethodParameter.Identifier.Text;

        public string SpeakMethodParameterDefaultValue =>
            _speakMethodParameter.Default.Value.ToFullString();

        public string TwoFerVariableName =>
            _twoFerVariable.Identifier.Text;

        public bool AssignsToParameter() =>
            _speakMethodParameter != null && 
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

        public bool MissingSpeakMethod() =>
            _speakMethod == null;

        public bool InvalidSpeakMethod() =>
            _twoFerClass.GetMethods("Speak").All(
                speakMethod => 
                    speakMethod.ParameterList.Parameters.Count != 1 ||
                    !speakMethod.ParameterList.Parameters[0].Type.IsEquivalentWhenNormalized(
                        PredefinedType(Token(SyntaxKind.StringKeyword))));

        public bool UsesOverloads() =>
            _twoFerClass.GetMethods("Speak").Count() > 1;

        public bool UsesDuplicateString()
        {
            var literalExpressionCount = _speakMethod
                .DescendantNodes<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("One for"));

            var interpolatedStringTextCount = _speakMethod
                .DescendantNodes<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("One for"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        public bool UsesStringJoin() =>
            _speakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Join")));

        public bool UsesStringConcat() =>
            _speakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Concat")));

        public bool UsesStringReplace() =>
            _speakMethod.InvokesMethod(IdentifierName("Replace"));

        public bool NoDefaultValue() =>
            _speakMethodParameter != null &&
            _speakMethod?.ParameterList != null &&
            _speakMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        public bool UsesInvalidDefaultValue() =>
            UseDefaultValue() &&
            !DefaultValueIsNull() &&
            !DefaultValueIsYouString() &&
            !DefaultValueIsYouStringSpecifiedAsConst();

        private bool UseDefaultValue() =>
            _speakMethodParameter?.Default != null;

        private bool DefaultValueIsNull() =>
            _speakMethodParameter.Default.Value.IsEquivalentWhenNormalized(NullLiteralExpression());

        private bool DefaultValueIsYouString() =>
            _speakMethodParameter.Default.Value.IsEquivalentWhenNormalized(StringLiteralExpression("you"));

        private bool DefaultValueIsYouStringSpecifiedAsConst() =>
            _speakMethodParameter.Default.Value is IdentifierNameSyntax identifierName &&
            _twoFerClass.AssignedVariableWithName(identifierName).IsEquivalentWhenNormalized(
                SyntaxFactory.VariableDeclarator(identifierName.Identifier, default, EqualsValueClause(StringLiteralExpression("you"))));
    }
}