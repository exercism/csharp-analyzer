using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerSyntax
    {
        public static bool MissingNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod == null;

        public static bool InvalidNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.Count != 1 ||
            !twoFerSolution.InputParameter.Type.IsEquivalentToNormalized(
                PredefinedType(Token(SyntaxKind.StringKeyword)));

        public static bool UsesOverloads(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.TwoFerClass.GetMethods("Name").Count() > 1;

        public static bool UsesDuplicateString(this TwoFerSolution twoFerSolution)
        {
            var literalExpressionCount = twoFerSolution.NameMethod
                .DescendantNodes<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("one for me"));

            var interpolatedStringTextCount = twoFerSolution.NameMethod
                .DescendantNodes<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("one for me"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        public static bool UsesStringJoin(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                IdentifierName("Join"));

        public static bool UsesStringConcat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                IdentifierName("Concat"));

        public static bool UsesStringReplace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression =>
                    invocationExpression.Expression is MemberAccessExpressionSyntax memberAccessExpression &&
                    memberAccessExpression.Name.IsEquivalentToNormalized(IdentifierName("Replace")));

        public static bool AssignsToParameter(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.AssignsToParameter(twoFerSolution.InputParameter);

        public static bool NoDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        public static bool UsesInvalidDefaultValue(this TwoFerSolution twoFerSolution) =>
            !twoFerSolution.UsesNullAsDefaultValue() && !twoFerSolution.UsesYouStringAsDefaultValue();

        public static bool UsesStringConcatenation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnedExpression.IsDefaultStringConcatenationExpression(twoFerSolution.InputParameter) ||
            twoFerSolution.ReturnedExpression.IsNullCoalescingStringConcatenationExpression(twoFerSolution.InputParameter) ||
            twoFerSolution.ReturnedExpression.IsTernaryOperatorStringConcatenationExpression(twoFerSolution.InputParameter);
        
        public static bool IsDefaultStringConcatenationExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerStringConcatenationExpression(
                    IdentifierName(inputParameter.Identifier)));

        public static bool UsesStringFormat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.ReturnedExpression.IsDefaultStringFormatExpression(twoFerSolution.InputParameter) ||
            twoFerSolution.ReturnedExpression.IsTernaryOperatorStringFormatExpression(twoFerSolution.InputParameter) ||
            twoFerSolution.ReturnedExpression.IsNullCoalescingStringFormatExpression(twoFerSolution.InputParameter);

        public static bool IsDefaultStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    IdentifierName(inputParameter.Identifier)));

        public static bool IsTernaryOperatorStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    TwoFerTernaryOperatorConditionalExpression(inputParameter))) ||
            expression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerStringInvocationExpressionOnParameter("IsNullOrEmpty", inputParameter),
                        IdentifierName(inputParameter.Identifier)))) ||
            expression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    TwoFerConditionalExpression(
                        TwoFerStringInvocationExpressionOnParameter("IsNullOrWhiteSpace", inputParameter),
                        IdentifierName(inputParameter.Identifier))));

        public static bool IsNullCoalescingStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerStringFormatInvocationExpression(
                    TwoFerCoalesceExpression(
                        IdentifierName(inputParameter.Identifier))));

        public static bool IsDefaultInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    IdentifierName(inputParameter.Identifier)));

        public static bool IsTernaryOperatorInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                IdentifierName(inputParameter.Identifier),
                                LiteralExpression(
                                    SyntaxKind.NullLiteralExpression)),
                            IdentifierName(inputParameter.Identifier)))));


        public static bool IsNullCoalescingInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    TwoFerCoalesceExpression(
                        IdentifierName(parameter.Identifier))));

        public static bool IsIsNullOrEmptyInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(TwoFerStringInvocationExpressionOnParameter("IsNullOrEmpty", parameter), IdentifierName("input")))));

        public static bool IsIsNullOrWhiteSpaceInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerInterpolatedStringExpression(
                    ParenthesizedExpression(
                        TwoFerConditionalExpression(TwoFerStringInvocationExpressionOnParameter("IsNullOrWhiteSpace", parameter), IdentifierName("input")))));

        public static bool IsNullCoalescingStringConcatenationExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerCoalesceExpression(
                            IdentifierName(inputParameter.Identifier)))));

        public static bool IsTernaryOperatorStringConcatenationExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsEquivalentToNormalized(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
                        TwoFerTernaryOperatorConditionalExpression(inputParameter)))) ||
            expression.IsEquivalentToNormalized(
                TwoFerStringConcatenationExpression(
                    ParenthesizedExpression(
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
                                                IdentifierName(inputParameter.Identifier))))),
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal("you")),
                            IdentifierName(inputParameter.Identifier))))) ||
                    expression.IsEquivalentToNormalized(
                        TwoFerStringConcatenationExpression(
                            ParenthesizedExpression(
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
                                                        IdentifierName(inputParameter.Identifier))))),
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal("you")),
                                    IdentifierName(inputParameter.Identifier)))));
        
        public static VariableDeclaratorSyntax AssignedVariable(this MethodDeclarationSyntax nameMethod)
        {
            if (nameMethod == null ||
                nameMethod.Body == null ||
                nameMethod.Body.Statements.Count != 2)
                return null;

            if (!(nameMethod.Body.Statements[1] is ReturnStatementSyntax) ||
                !(nameMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration))
                return null;
            
            if (localDeclaration.Declaration.Variables.Count != 1 ||
                !localDeclaration.Declaration.Type.IsEquivalentToNormalized(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
                !localDeclaration.Declaration.Type.IsEquivalentToNormalized(IdentifierName("var")))
                return null;

            return localDeclaration.Declaration.Variables[0];
        }
        
        public static bool UsesNullAsDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.InputParameter.Default.Value.IsEquivalentToNormalized(
                LiteralExpression(
                    SyntaxKind.NullLiteralExpression));

        public static bool UsesYouStringAsDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.InputParameter.Default.Value.IsEquivalentToNormalized(
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")));

        public static InvocationExpressionSyntax TwoFerStringInvocationExpressionOnParameter(string methodName, ParameterSyntax parameter) =>
            InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword)),
                        IdentifierName(methodName)))
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                IdentifierName(parameter.Identifier)))));

        public static BinaryExpressionSyntax TwoFerCoalesceExpression(IdentifierNameSyntax identifierName) =>
            BinaryExpression(
                SyntaxKind.CoalesceExpression,
                identifierName,
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")));

        public static InterpolatedStringExpressionSyntax TwoFerInterpolatedStringExpression(ExpressionSyntax interpolationExpression) =>
            InterpolatedStringExpression(
                    Token(SyntaxKind.InterpolatedStringStartToken))
                .WithContents(
                    List(
                        new InterpolatedStringContentSyntax[]
                        {
                            InterpolatedStringText(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.InterpolatedStringTextToken,
                                    "One for ",
                                    "One for ",
                                    TriviaList())),
                            Interpolation(interpolationExpression),
                            InterpolatedStringText(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.InterpolatedStringTextToken,
                                    ", one for me.",
                                    ", one for me.",
                                    TriviaList()))
                        }));

        public static ConditionalExpressionSyntax TwoFerConditionalExpression(ExpressionSyntax condition, IdentifierNameSyntax identifierName) =>
            ConditionalExpression(
                condition,
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")),
                identifierName);

        public static ConditionalExpressionSyntax TwoFerTernaryOperatorConditionalExpression(ParameterSyntax inputParameter) =>
            TwoFerConditionalExpression(
                BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    IdentifierName(inputParameter.Identifier),
                    LiteralExpression(
                        SyntaxKind.NullLiteralExpression)),
                IdentifierName(inputParameter.Identifier));

        public static BinaryExpressionSyntax TwoFerStringConcatenationExpression(ExpressionSyntax middleExpression) =>
            BinaryExpression(
                SyntaxKind.AddExpression,
                BinaryExpression(
                    SyntaxKind.AddExpression,
                    LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal("One for ")),
                    middleExpression),
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal(", one for me.")));

        public static InvocationExpressionSyntax TwoFerStringFormatInvocationExpression(ExpressionSyntax argumentExpression) =>
            InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword)),
                        IdentifierName("Format")))
                .WithArgumentList(
                    ArgumentList(
                        SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]{
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal("One for {0}, one for me."))),
                                Token(SyntaxKind.CommaToken),
                                Argument(argumentExpression)})));
        
        public static bool AssignsVariableUsingKnownInitializer(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.VariableAssignedUsingNullCoalescingOperator() ||
            twoFerSolution.VariableAssignedUsingNullCheck() ||
            twoFerSolution.VariableAssignedUsingIsNullOrEmpty() ||
            twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace();

        public static bool AssignsToParameterUsingKnownCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsToParameterUsingNullCoalescingCheck() ||
            twoFerSolution.AssignsToParameterUsingNullCheck() ||
            twoFerSolution.AssignsToParameterUsingIsNullOrEmptyCheck() ||
            twoFerSolution.AssignsToParameterUsingIsNullOrWhiteSpaceCheck();
        
        public static bool AssignsToParameterUsingNullCoalescingCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.Body.Statements[0].IsEquivalentToNormalized(
                ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(twoFerSolution.InputParameter.Identifier),
                        BinaryExpression(
                            SyntaxKind.CoalesceExpression,
                            IdentifierName("input"),
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal("you"))))));
        
        public static bool AssignsToParameterUsingNullCheck(this TwoFerSolution twoFerSolution) =>
        twoFerSolution.NameMethod.Body.Statements[0].IsEquivalentToNormalized(
            IfStatement(
                ParameterIsNullExpression(twoFerSolution),
                TwoFerAssignParameterToYou(twoFerSolution)));
        
        public static bool AssignsToParameterUsingIsNullOrEmptyCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.Body.Statements[0].IsEquivalentToNormalized(
                IfStatement(
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
                    TwoFerAssignParameterToYou(twoFerSolution)));
        
        public static bool AssignsToParameterUsingIsNullOrWhiteSpaceCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.Body.Statements[0].IsEquivalentToNormalized(
                IfStatement(
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
                        TwoFerAssignParameterToYou(twoFerSolution)));

        private static BlockSyntax TwoFerAssignParameterToYou(TwoFerSolution twoFerSolution) =>
            Block(
                SingletonList<StatementSyntax>(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(twoFerSolution.InputParameter.Identifier),
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal("you"))))));

        private static BinaryExpressionSyntax ParameterIsNullExpression(TwoFerSolution twoFerSolution) =>
            BinaryExpression(
                SyntaxKind.EqualsExpression,
                IdentifierName(twoFerSolution.InputParameter.Identifier),
                LiteralExpression(
                    SyntaxKind.NullLiteralExpression));


        public static bool VariableAssignedUsingNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentToNormalized(
                EqualsValueClause(
                    TwoFerCoalesceExpression(
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

        public static bool VariableAssignedUsingNullCheck(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentToNormalized(
                EqualsValueClause(
                    TwoFerConditionalExpression(
                        BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            IdentifierName(twoFerSolution.InputParameter.Identifier),
                            LiteralExpression(
                                SyntaxKind.NullLiteralExpression)), 
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

        public static bool VariableAssignedUsingIsNullOrEmpty(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentToNormalized(
                EqualsValueClause(
                    TwoFerConditionalExpression(
                        TwoFerStringInvocationExpressionOnParameter("IsNullOrEmpty", twoFerSolution.InputParameter),
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

        public static bool VariableAssignedUsingIsNullOrWhiteSpace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsEquivalentToNormalized(
                EqualsValueClause(
                    TwoFerConditionalExpression(
                        TwoFerStringInvocationExpressionOnParameter("IsNullOrWhiteSpace", twoFerSolution.InputParameter),
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));
    }
}