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
        public TwoFerSolution(Solution solution) : base(solution)
        {
        }

        private ClassDeclarationSyntax TwoFerClass =>
            SyntaxRoot.GetClass("TwoFer");

        private MethodDeclarationSyntax SpeakMethod =>
            TwoFerClass?.GetMethod("Speak");

        private ParameterSyntax SpeakMethodParameter =>
            SpeakMethod?.ParameterList.Parameters.FirstOrDefault();

        private ExpressionSyntax TwoFerExpression =>
            SpeakMethod?.ReturnedExpression();

        private VariableDeclaratorSyntax TwoFerVariable
        {
            get
            {
                var speakMethod = SpeakMethod;
                if (speakMethod == null ||
                    speakMethod.Body == null ||
                    speakMethod.Body.Statements.Count != 2)
                    return null;

                if (!(speakMethod.Body.Statements[1] is ReturnStatementSyntax) ||
                    !(speakMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration))
                    return null;

                if (localDeclaration.Declaration.Variables.Count != 1 ||
                    !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
                    !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(IdentifierName("var")))
                    return null;

                return localDeclaration.Declaration.Variables[0];
            }
        }

        public string SpeakMethodName =>
            SpeakMethod.Identifier.Text;

        public string SpeakMethodParameterName =>
            SpeakMethodParameter.Identifier.Text;

        public string SpeakMethodParameterDefaultValue =>
            SpeakMethodParameter.Default.Value.ToFullString();

        public string TwoFerVariableName =>
            TwoFerVariable.Identifier.Text;

        public bool AssignsToParameter =>
            SpeakMethodParameter != null &&
            SpeakMethod.AssignsToParameter(SpeakMethodParameter);

        public bool UsesSingleLine =>
            SpeakMethod.SingleLine();

        public bool UsesExpressionBody =>
            SpeakMethod.IsExpressionBody();

        public bool UsesStringConcatenation =>
            ReturnsStringConcatenationWithDefaultValue ||
            ReturnsStringConcatenationWithNullCoalescingOperator ||
            ReturnsStringConcatenationWithTernaryOperator ||
            ReturnsStringConcatenationWithVariable;

        private bool ReturnsStringConcatenationWithDefaultValue =>
            Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerParameterIdentifierName(this)));

        private bool ReturnsStringConcatenationWithNullCoalescingOperator =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            TwoFerParameterIdentifierName(this)))));

        private bool ReturnsStringConcatenationWithTernaryOperator =>
            ReturnsStringConcatenationWithNullCheck ||
            ReturnsStringConcatenationWithIsNullOrEmptyCheck ||
            ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck;

        private bool ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                            TwoFerParameterIdentifierName(this)))));

        private bool ReturnsStringConcatenationWithIsNullOrEmptyCheck =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(
                            TwoFerIsNullOrEmptyInvocationExpression(this),
                            TwoFerParameterIdentifierName(this)))));

        private bool ReturnsStringConcatenationWithNullCheck =>
            Returns(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpressionWithNullCheck(this))));

        public bool UsesStringFormat =>
            ReturnsStringFormatWithDefaultValue ||
            ReturnsStringFormatWithNullCoalescingOperator ||
            ReturnsStringFormatWithTernaryOperator ||
            ReturnsStringFormatWithVariable;

        private bool ReturnsStringFormatWithDefaultValue =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerParameterIdentifierName(this)));

        private bool ReturnsStringFormatWithNullCoalescingOperator =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(this))));

        private bool ReturnsStringFormatWithTernaryOperator =>
            ReturnsStringFormatWithNullCheck ||
            ReturnsStringFormatWithIsNullOrEmptyCheck ||
            ReturnsStringFormatWithIsNullOrWhiteSpaceCheck;

        private bool ReturnsStringFormatWithIsNullOrWhiteSpaceCheck =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                        TwoFerParameterIdentifierName(this))));

        private bool ReturnsStringFormatWithIsNullOrEmptyCheck =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerIsNullOrEmptyInvocationExpression(this),
                        TwoFerParameterIdentifierName(this))));

        private bool ReturnsStringFormatWithNullCheck =>
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpressionWithNullCheck(this)));

        public bool ReturnsStringInterpolation =>
            UsesStringInterpolationWithDefaultValue ||
            UsesStringInterpolationWithNullCheck ||
            UsesStringInterpolationWithNullCoalescingOperator ||
            UsesStringInterpolationWithIsNullOrEmptyCheck ||
            UsesStringInterpolationWithIsNullOrWhiteSpaceCheck;

        public bool UsesStringInterpolationWithDefaultValue =>
            Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerParameterIdentifierName(this)));

        public bool UsesStringInterpolationWithNullCheck =>
            Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    EqualsExpression(
                        TwoFerParameterIdentifierName(this),
                        NullLiteralExpression()),
                    TwoFerParameterIdentifierName(this)));

        public bool UsesStringInterpolationWithNullCoalescingOperator =>
            Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(this))));

        private bool UsesStringInterpolationWithIsNullOrEmptyCheck =>
            Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    TwoFerIsNullOrEmptyInvocationExpression(this),
                    TwoFerParameterIdentifierName(this)));

        private bool UsesStringInterpolationWithIsNullOrWhiteSpaceCheck =>
            Returns(
                TwoFerConditionalInterpolatedStringExpression(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                    TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingKnownExpression =>
            AssignsParameterUsingNullCoalescingOperator ||
            AssignsParameterUsingNullCheck ||
            AssignsParameterUsingIfNullCheck ||
            AssignsParameterUsingIsNullOrEmptyCheck ||
            AssignsParameterUsingIfIsNullOrEmptyCheck ||
            AssignsParameterUsingIsNullOrWhiteSpaceCheck ||
            AssignsParameterUsingIfIsNullOrWhiteSpaceCheck;

        public bool AssignsParameterUsingNullCoalescingOperator =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerCoalesceExpression(
                        TwoFerParameterIdentifierName(this)),
                    TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingNullCheck =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerParameterIsNullConditionalExpression(this),
                    TwoFerParameterIdentifierName(this)));

        public bool AssignsParameterUsingIfNullCheck =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                    TwoFerParameterIsNullExpression(this),
                    TwoFerParameterIdentifierName(this)));

        private bool AssignsParameterUsingIsNullOrEmptyCheck =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerParameterIsNullOrEmptyConditionalExpression(this),
                    TwoFerParameterIdentifierName(this)));

        private bool AssignsParameterUsingIfIsNullOrEmptyCheck =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                    TwoFerIsNullOrEmptyInvocationExpression(this),
                    TwoFerParameterIdentifierName(this)));

        private bool AssignsParameterUsingIsNullOrWhiteSpaceCheck =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterStatement(
                    TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(this),
                    TwoFerParameterIdentifierName(this)));

        private bool AssignsParameterUsingIfIsNullOrWhiteSpaceCheck =>
            ParameterAssignedUsingStatement(
                TwoFerAssignParameterIfStatement(
                    TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
                    TwoFerParameterIdentifierName(this)));

        private bool ParameterAssignedUsingStatement(SyntaxNode statement) =>
            AssignsStatement &&
            AssignmentStatement.IsEquivalentWhenNormalized(statement);

        private bool AssignsStatement =>
            AssignmentStatement != null;

        private StatementSyntax AssignmentStatement =>
            SpeakMethod.Body?.Statements[0];

        public bool AssignsVariable =>
            TwoFerVariable != null;

        public bool AssignsVariableUsingKnownInitializer =>
            AssignsVariableUsingNullCoalescingOperator ||
            AssignsVariableUsingNullCheck ||
            AssignsVariableUsingIsNullOrEmptyCheck ||
            AssignsVariableUsingIsNullOrWhiteSpaceCheck;

        public bool AssignsVariableUsingNullCoalescingOperator =>
            AssignsVariableUsingExpression(
                TwoFerCoalesceExpression(
                    TwoFerParameterIdentifierName(this)));

        private bool AssignsVariableUsingNullCheck =>
            AssignsVariableUsingExpression(TwoFerParameterIsNullConditionalExpression(this));

        private bool AssignsVariableUsingIsNullOrEmptyCheck =>
            AssignsVariableUsingExpression(TwoFerParameterIsNullOrEmptyConditionalExpression(this));

        private bool AssignsVariableUsingIsNullOrWhiteSpaceCheck =>
            AssignsVariableUsingExpression(TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(this));

        private bool AssignsVariableUsingExpression(ExpressionSyntax initializer) =>
            AssignsVariable &&
            TwoFerVariable.Initializer.IsEquivalentWhenNormalized(
                EqualsValueClause(initializer));

        public bool ReturnsStringInterpolationWithVariable =>
            AssignsVariable &&
            Returns(
                TwoFerInterpolatedStringExpression(
                    TwoFerVariableIdentifierName(this)));

        private bool ReturnsStringConcatenationWithVariable =>
            AssignsVariable &&
            Returns(
                TwoFerStringConcatenationExpression(
                    TwoFerVariableIdentifierName(this)));

        private bool ReturnsStringFormatWithVariable =>
            AssignsVariable &&
            Returns(
                TwoFerStringFormatInvocationExpression(
                    TwoFerVariableIdentifierName(this)));

        private bool Returns(SyntaxNode returned) =>
            TwoFerExpression.IsEquivalentWhenNormalized(returned);

        public bool MissingSpeakMethod =>
            SpeakMethod == null;

        public bool InvalidSpeakMethod =>
            TwoFerClass.GetMethods("Speak").All(
                speakMethod =>
                    speakMethod.ParameterList.Parameters.Count != 1 ||
                    !speakMethod.ParameterList.Parameters[0].Type.IsEquivalentWhenNormalized(
                        PredefinedType(Token(SyntaxKind.StringKeyword))));

        public bool UsesOverloads =>
                TwoFerClass.GetMethods("Speak").Count() > 1;

        public bool UsesDuplicateString
        {
            get
            {
                var literalExpressionCount = SpeakMethod
                    .DescendantNodes<LiteralExpressionSyntax>()
                    .Count(literalExpression => literalExpression.Token.ValueText.Contains("One for"));

                var interpolatedStringTextCount = SpeakMethod
                    .DescendantNodes<InterpolatedStringTextSyntax>()
                    .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("One for"));

                return literalExpressionCount + interpolatedStringTextCount > 1;
            }
        }

        public bool UsesStringJoin =>
            SpeakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Join")));

        public bool UsesStringConcat =>
            SpeakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Concat")));

        public bool UsesStringReplace =>
            SpeakMethod.InvokesMethod(IdentifierName("Replace"));

        public bool NoDefaultValue =>
            SpeakMethodParameter != null &&
            SpeakMethod?.ParameterList != null &&
            SpeakMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        public bool UsesInvalidDefaultValue =>
            UsesDefaultValue &&
            !DefaultValueIsNull &&
            !DefaultValueIsYouString &&
            !DefaultValueIsYouStringSpecifiedAsConst;

        private bool UsesDefaultValue =>
            SpeakMethodParameter?.Default != null;

        private bool DefaultValueIsNull =>
            SpeakMethodParameter.Default.Value.IsEquivalentWhenNormalized(NullLiteralExpression());

        private bool DefaultValueIsYouString =>
            SpeakMethodParameter.Default.Value.IsEquivalentWhenNormalized(StringLiteralExpression("you"));

        private bool DefaultValueIsYouStringSpecifiedAsConst =>
            SpeakMethodParameter.Default.Value is IdentifierNameSyntax identifierName &&
            TwoFerClass.AssignedVariableWithName(identifierName).IsEquivalentWhenNormalized(
                SyntaxFactory.VariableDeclarator(identifierName.Identifier, default, EqualsValueClause(StringLiteralExpression("you"))));

        public bool UsesIsNullOrEmptyCheck =>
            UsesStringInterpolationWithIsNullOrEmptyCheck ||
            AssignsParameterUsingIsNullOrEmptyCheck ||
            AssignsParameterUsingIfIsNullOrEmptyCheck ||
            AssignsVariableUsingIsNullOrEmptyCheck;

        public bool UsesIsNullOrWhiteSpaceCheck =>
            UsesStringInterpolationWithIsNullOrWhiteSpaceCheck ||
            AssignsVariableUsingIsNullOrWhiteSpaceCheck ||
            AssignsParameterUsingIsNullOrWhiteSpaceCheck ||
            AssignsParameterUsingIfIsNullOrWhiteSpaceCheck;

        public bool UsesNullCheck =>
            UsesStringInterpolationWithNullCheck ||
            AssignsParameterUsingNullCheck ||
            AssignsParameterUsingIfNullCheck ||
            AssignsVariableUsingNullCheck;
    }
}