using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSolutionParser
    {
        public static TwoFerSolution Parse(ParsedSolution solution)
        {
            var twoFerClass = solution.SyntaxRoot.GetClass("TwoFer");
            var speakMethod = twoFerClass.GetMethod("Speak");
            var speakMethodParameter = speakMethod?.ParameterList.Parameters.FirstOrDefault();
            var speakMethodReturnedExpression = speakMethod?.ReturnedExpression();
            var speakMethodVariable = speakMethod?.AssignedVariable();
            var twoFerError = ToTwoFerError(twoFerClass, speakMethod, speakMethodParameter);
            var twoFerStringConcatenationExpression = TwoFerStringConcatenationExpression(speakMethod);
            var twoFerStringInterpolationExpression = TwoFerStringInterpolationExpression(speakMethod);
            var twoFerStringFormatExpression = TwoFerStringFormatExpression(speakMethod);
  
            var twoFerExpression =
                twoFerStringConcatenationExpression ??
                twoFerStringInterpolationExpression ??
                twoFerStringFormatExpression;

            var twoFerValueExpression = TwoFerValueExpression(twoFerStringConcatenationExpression, twoFerStringInterpolationExpression,
                twoFerStringFormatExpression);
            
            var twoFerArgumentVariable = twoFerClass.ArgumentVariable(twoFerValueExpression);
            var twoFerArgumentVariableFieldDeclaration = twoFerArgumentVariable.FieldDeclaration();
            var twoFerArgumentVariableLocalDeclarationStatement = twoFerArgumentVariable.LocalDeclarationStatement();
            
            var speakMethodReturnType = ReturnedAs(twoFerExpression, speakMethodReturnedExpression, speakMethodParameter);

            var twoFerArgumentType = ArgumentDefinedAs(twoFerArgumentVariableFieldDeclaration, twoFerArgumentVariableLocalDeclarationStatement, twoFerValueExpression);
            var twoFerArgumentValueExpression = ArgumentValueExpression(twoFerArgumentType, twoFerValueExpression, twoFerArgumentVariable);

            var formattingType = ToTwoFerFormattingType(
                twoFerStringConcatenationExpression,
                twoFerStringInterpolationExpression,
                twoFerStringFormatExpression);

            var twoFerValueType = ToTwoFerValueType(twoFerArgumentValueExpression, speakMethodParameter);

            return new TwoFerSolution(solution, twoFerClass, speakMethod, speakMethodParameter, speakMethodReturnedExpression, speakMethodVariable, twoFerError);
        }

        private static ExpressionSyntax TwoFerValueExpression(ExpressionSyntax twoFerStringConcatenationExpression, ExpressionSyntax twoFerStringInterpolationExpression, ExpressionSyntax twoFerStringFormatExpression)
        {
            if (twoFerStringFormatExpression is InvocationExpressionSyntax invocationExpression)
                return invocationExpression.ArgumentList.Arguments[1].Expression;

            if (twoFerStringConcatenationExpression is BinaryExpressionSyntax binaryExpression &&
                binaryExpression.Left is BinaryExpressionSyntax left)
                return left.Right;

            if (twoFerStringInterpolationExpression is InterpolatedStringExpressionSyntax interpolatedStringExpression &&
                interpolatedStringExpression.Contents[1] is InterpolationSyntax interpolation)
                return interpolation.Expression;

            return null;
        }

        private static TwoFerValueType ToTwoFerValueType(ExpressionSyntax expression, ParameterSyntax speakMethodParameter)
        {
            if (expression.IsEquivalentWhenNormalized(
                    IdentifierName(speakMethodParameter)) &&
                speakMethodParameter.Default.IsEquivalentWhenNormalized(
                    EqualsValueClause(
                        StringLiteralExpression("you"))))
                return TwoFerValueType.DefaultValue;
            
            if (expression.IsEquivalentWhenNormalized(
                ConditionalExpression(
                    BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        IdentifierName(speakMethodParameter),
                        LiteralExpression(
                            SyntaxKind.NullLiteralExpression)),
                    StringLiteralExpression("you"),
                    IdentifierName(speakMethodParameter))))
                return TwoFerValueType.NullCheck;

            if (expression.IsEquivalentWhenNormalized(
                ConditionalExpression(
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                PredefinedType(
                                    Token(SyntaxKind.StringKeyword)),
                                IdentifierName("IsNullOrEmpty")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(
                                        IdentifierName("input"))))),
                    LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal("you")),
                    IdentifierName(speakMethodParameter))))
                return TwoFerValueType.IsNullOrEmptyCheck;

            if (expression.IsEquivalentWhenNormalized(
                ConditionalExpression(
                    InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                PredefinedType(
                                    Token(SyntaxKind.StringKeyword)),
                                IdentifierName("IsNullOrWhiteSpace")))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(
                                        IdentifierName("input"))))),
                    LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal("you")),
                    IdentifierName(speakMethodParameter))))
                return TwoFerValueType.IsNullOrWhiteSpaceCheck;

            if (expression is BinaryExpressionSyntax binaryExpression &&
                binaryExpression.Kind() == SyntaxKind.CoalesceExpression &&
                binaryExpression.Left.IsEquivalentWhenNormalized(
                    IdentifierName(speakMethodParameter)) &&
                binaryExpression.Right.IsEquivalentWhenNormalized(StringLiteralExpression("you")))
                return TwoFerValueType.NullCoalescingOperator;
            
            return TwoFerValueType.Unknown;
        }

        private static TwoFerFormattingType ToTwoFerFormattingType(ExpressionSyntax stringConcatenationValueExpression, ExpressionSyntax stringInterpolationValueExpression, ExpressionSyntax stringFormatValueExpression)
        {
            if (stringConcatenationValueExpression != null)
                return TwoFerFormattingType.StringConcatenation;

            if (stringInterpolationValueExpression != null)
                return TwoFerFormattingType.StringInterpolation;

            if (stringFormatValueExpression != null)
                return TwoFerFormattingType.StringFormat;

            return TwoFerFormattingType.Unknown;
        }

        private static ExpressionSyntax TwoFerStringInterpolationExpression(MethodDeclarationSyntax speakMethod) =>
            speakMethod
                .DescendantNodes<InterpolatedStringExpressionSyntax>()
                .FirstOrDefault(interpolatedStringExpression =>
                    interpolatedStringExpression.Contents.Count == 3 &&
                    interpolatedStringExpression.Contents[0].IsEquivalentWhenNormalized(InterpolatedStringText("One for ")) &&
                    interpolatedStringExpression.Contents[1] is InterpolationSyntax &&
                    interpolatedStringExpression.Contents[2].IsEquivalentWhenNormalized(InterpolatedStringText(", one for me.")));

        private static ExpressionSyntax TwoFerStringFormatExpression(MethodDeclarationSyntax speakMethod) =>
            speakMethod
                .DescendantNodes<InvocationExpressionSyntax>()
                .FirstOrDefault(invocationExpression =>
                    invocationExpression.Expression.IsEquivalentWhenNormalized(
                        StringMemberAccessExpression(
                            IdentifierName("Format"))) &&
                    invocationExpression.ArgumentList.Arguments.Count == 2 &&
                    invocationExpression.ArgumentList.Arguments[0].Expression.IsEquivalentWhenNormalized(
                        StringLiteralExpression("One for {0}, one for me.")));

        private static ExpressionSyntax TwoFerStringConcatenationExpression(MethodDeclarationSyntax speakMethod) =>
            speakMethod
                .DescendantNodes<BinaryExpressionSyntax>()
                .FirstOrDefault(binaryExpression =>
                    binaryExpression.Kind() == SyntaxKind.AddExpression &&
                    binaryExpression.Left is BinaryExpressionSyntax left &&
                    left.Left.IsEquivalentWhenNormalized(StringLiteralExpression("One for "))
                    && binaryExpression.Right.IsEquivalentWhenNormalized(StringLiteralExpression(", one for me.")));

        private static TwoFerError ToTwoFerError(ClassDeclarationSyntax twoFerClass, MethodDeclarationSyntax speakMethod, ParameterSyntax speakMethodParameter)
        {
            if (twoFerClass.UsesOverloads())
                return TwoFerError.UsesOverloads;

            if (speakMethod.MissingSpeakMethod())
                return TwoFerError.MissingSpeakMethod;
                
            if (speakMethod.InvalidSpeakMethod(speakMethodParameter))
                return TwoFerError.InvalidSpeakMethod;

            if (speakMethod.UsesDuplicateString())
                return TwoFerError.UsesDuplicateString;

            if (speakMethod.NoDefaultValue())
                return TwoFerError.NoDefaultValue;

            if (speakMethodParameter.UsesInvalidDefaultValue())
                return TwoFerError.InvalidDefaultValue;

            if (speakMethod.UsesStringReplace())
                return TwoFerError.UsesStringReplace;

            if (speakMethod.UsesStringJoin())
                return TwoFerError.UsesStringJoin;

            if (speakMethod.UsesStringConcat())
                return TwoFerError.UsesStringConcat;

            return TwoFerError.None;
        }

        private static bool MissingSpeakMethod(this MethodDeclarationSyntax speakMethod) =>
            speakMethod == null;

        private static bool InvalidSpeakMethod(this MethodDeclarationSyntax speakMethod, ParameterSyntax speakMethodParameter) =>
            speakMethod.ParameterList.Parameters.Count != 1 ||
            !speakMethodParameter.Type.IsEquivalentWhenNormalized(
                PredefinedType(Token(SyntaxKind.StringKeyword)));

        private static bool UsesOverloads(this ClassDeclarationSyntax twoFerClass) =>
            twoFerClass.GetMethods("Speak").Count() > 1;

        private static bool UsesDuplicateString(this MethodDeclarationSyntax speakMethod)
        {
            var literalExpressionCount = speakMethod
                .DescendantNodes<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("One for"));

            var interpolatedStringTextCount = speakMethod
                .DescendantNodes<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("One for"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        private static bool UsesStringJoin(this MethodDeclarationSyntax speakMethod) =>
            speakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Join")));

        private static bool UsesStringConcat(this MethodDeclarationSyntax speakMethod) =>
            speakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Concat")));

        private static bool UsesStringReplace(this MethodDeclarationSyntax methodDeclarationSyntax) =>
            methodDeclarationSyntax.InvokesMethod(IdentifierName("Replace"));

        private static bool NoDefaultValue(this MethodDeclarationSyntax speakMethod) =>
            speakMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        private static bool UsesInvalidDefaultValue(this ParameterSyntax speakMethodParameter) =>
            !speakMethodParameter.Default.Value.IsEquivalentWhenNormalized(NullLiteralExpression()) &&
            !speakMethodParameter.Default.Value.IsEquivalentWhenNormalized(StringLiteralExpression("you"));
    }
}