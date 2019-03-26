using System;
using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Rewriting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFerComments;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerAnalyzer
    {   
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            var twoFerSolution = new TwoFerSolution(parsedSolution);
            
            if (twoFerSolution.UsesOverloads())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingNameMethod() ||
                twoFerSolution.InvalidNameMethod())
                return twoFerSolution.DisapproveWithComment(FixCompileErrors);
            
            if (twoFerSolution.UsesDuplicateString())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue())
                return twoFerSolution.DisapproveWithComment(UseDefaultValue);

            if (twoFerSolution.UsesInvalidDefaultValue())
                return twoFerSolution.DisapproveWithComment(InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin())
                return parsedSolution.DisapproveWithComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringConcat);

            if (twoFerSolution.AssignsToParameter())
                return twoFerSolution.DisapproveWithComment(DontAssignToParameter);

            // TODO: simplify further
            
            if (twoFerSolution.NameMethod.IsExpressionBody() &&
                twoFerSolution.NameMethod.SingleStatementExpression().IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveAsOptimal();
            
            if (twoFerSolution.NameMethod.IsExpressionBody() &&
                twoFerSolution.NameMethod.SingleStatementExpression().IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.NameMethod.SingleStatementExpression().IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.NameMethod.SingleStatementExpression().IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (twoFerSolution.NameMethod.SingleStatementExpression().IsTernaryOperatorInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.UsesYouStringAsDefaultValue() &&
                twoFerSolution.NameMethod.SingleStatementExpression().IsDefaultStringConcatenationExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);
            
            if (twoFerSolution.UsesNullAsDefaultValue() &&
                twoFerSolution.NameMethod.SingleStatementExpression().IsNullCoalescingStringConcatenationExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);
            
            if (twoFerSolution.NameMethod.SingleStatementExpression().IsDefaultStringFormatExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.NameMethod.SingleStatementExpression().IsNullCoalescingStringFormatExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.AssignsVariableAndUsesThatInStringInterpolation())
                return twoFerSolution.ApproveAsOptimal();
            
            if (twoFerSolution.AssignsVariableAndUsesThatInStringFormat())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);
            
            if (twoFerSolution.AssignsVariableAndUsesThatInStringConcatenation())
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);
            
            return twoFerSolution.ReferToMentor();
        }

        private static bool MissingNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod == null;

        private static bool InvalidNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.Count != 1 ||
            !twoFerSolution.InputParameter.Type.IsSafeEquivalentTo(
                PredefinedType(Token(SyntaxKind.StringKeyword)));

        private static bool UsesOverloads(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.TwoFerClass.GetMethods("Name").Count() > 1;

        private static bool UsesDuplicateString(this TwoFerSolution twoFerSolution)
        {
            var literalExpressionCount = twoFerSolution.NameMethod
                .DescendantNodes<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("one for me"));

            var interpolatedStringTextCount = twoFerSolution.NameMethod
                .DescendantNodes<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("one for me"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        private static bool UsesStringJoin(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                IdentifierName("Join"));

        private static bool UsesStringConcat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                IdentifierName("Concat"));

        private static bool UsesStringReplace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod
                .DescendantNodes<InvocationExpressionSyntax>()
                .Any(invocationExpression =>
                        invocationExpression.Expression is MemberAccessExpressionSyntax memberAccessExpression &&
                        memberAccessExpression.Name.IsSafeEquivalentTo(IdentifierName("Replace")) &&
                        invocationExpression.ArgumentList.IsSafeEquivalentTo(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]{
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                Literal("you"))),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName(twoFerSolution.InputParameter.Identifier))}))));

        private static bool AssignsToParameter(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.AssignsToParameter(twoFerSolution.InputParameter);

        private static bool NoDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);

        private static bool UsesInvalidDefaultValue(this TwoFerSolution twoFerSolution) =>
            !twoFerSolution.UsesNullAsDefaultValue() && !twoFerSolution.UsesYouStringAsDefaultValue();

        private static bool UsesNullAsDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.InputParameter.Default.Value.IsSafeEquivalentTo(
                LiteralExpression(
                    SyntaxKind.NullLiteralExpression));

        private static bool UsesYouStringAsDefaultValue(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.InputParameter.Default.Value.IsSafeEquivalentTo(
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")));

        private static bool IsDefaultInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsInterpolatedStringExpression(
                Interpolation(
                    IdentifierName(inputParameter.Identifier)));

        private static bool IsTernaryOperatorInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsInterpolatedStringExpression(
                Interpolation(
                        CreateConditionalExpressionForInterpolation(IdentifierName(inputParameter.Identifier)))
                    .WithFormatClause(
                        CreateInterpolationFormatClauseForConditional()));

        private static bool IsNullCoalescingInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsInterpolatedStringExpression(
                Interpolation(
                    CreateCoalesceExpression(
                        IdentifierName(inputParameter.Identifier))));

        private static BinaryExpressionSyntax CreateCoalesceExpression(IdentifierNameSyntax identifierName) =>
            BinaryExpression(
                SyntaxKind.CoalesceExpression,
                identifierName,
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")));

        private static bool IsInterpolatedStringExpression(this ExpressionSyntax expression, InterpolationSyntax interpolation)
        {
            var triviaRemover = new TriviaRemoverSyntaxRewriter();
            return triviaRemover.Visit(expression).IsEquivalentTo(triviaRemover.Visit(CreateInterpolatedStringExpression(interpolation)));
        }

        private static InterpolatedStringExpressionSyntax CreateInterpolatedStringExpression(InterpolationSyntax interpolation) =>
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
                                interpolation,
                                InterpolatedStringText(
                                    Token(
                                        TriviaList(),
                                        SyntaxKind.InterpolatedStringTextToken,
                                        ", one for me.",
                                        ", one for me.",
                                        TriviaList()))
                            }));

        private static InterpolationFormatClauseSyntax CreateInterpolationFormatClauseForConditional() =>
            InterpolationFormatClause(
                    Token(SyntaxKind.ColonToken))
                .WithFormatStringToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.InterpolatedStringTextToken,
                        " input",
                        " input",
                        TriviaList()));

        private static ConditionalExpressionSyntax CreateConditionalExpressionForInterpolation(IdentifierNameSyntax identifierName) =>
            ConditionalExpression(
                BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    identifierName,
                    LiteralExpression(
                        SyntaxKind.NullLiteralExpression)),
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")),
                IdentifierName(
                    MissingToken(SyntaxKind.IdentifierToken)))
                .WithColonToken(
                    MissingToken(SyntaxKind.ColonToken).WithoutTrivia());

        private static ConditionalExpressionSyntax CreateConditionalExpression(IdentifierNameSyntax identifierName) =>
            ConditionalExpression(
                BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    identifierName,
                    LiteralExpression(
                        SyntaxKind.NullLiteralExpression)),
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")),
                identifierName);

        private static bool IsDefaultStringConcatenationExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringConcatenationExpression(
                    IdentifierName(inputParameter.Identifier)));

        private static BinaryExpressionSyntax CreateStringConcatenationExpression(ExpressionSyntax middleExpression) =>
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

        private static bool IsNullCoalescingStringConcatenationExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringConcatenationExpression(
                    ParenthesizedExpression(
                        CreateCoalesceExpression(
                            IdentifierName(inputParameter.Identifier)))));

        private static bool IsDefaultStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringFormatInvocationExpression(
                    IdentifierName(inputParameter.Identifier)));

        private static bool IsNullCoalescingStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringFormatInvocationExpression(
                    CreateCoalesceExpression(
                        IdentifierName(inputParameter.Identifier))));

        private static InvocationExpressionSyntax CreateStringFormatInvocationExpression(ExpressionSyntax argumentExpression) =>
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

        private static bool AssignsVariableAndUsesThatInStringConcatenation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableAndUsesThatInReturnStatement(
                (returnStatement, variable) =>
                    returnStatement.Expression.IsSafeEquivalentTo(
                        CreateStringConcatenationExpression(
                            IdentifierName(variable.Identifier))));

        private static bool AssignsVariableAndUsesThatInStringFormat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableAndUsesThatInReturnStatement(
                (returnStatement, variable) =>
                    returnStatement.Expression.IsSafeEquivalentTo(
                        CreateStringFormatInvocationExpression(
                            IdentifierName(variable.Identifier))));

        private static bool AssignsVariableAndUsesThatInStringInterpolation(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.AssignsVariableAndUsesThatInReturnStatement(
                (returnStatement, variable) =>
                    returnStatement.Expression.IsInterpolatedStringExpression(
                        Interpolation(
                            IdentifierName(variable.Identifier))));

        private static bool AssignsVariableAndUsesThatInReturnStatement(this TwoFerSolution twoFerSolution, Func<ReturnStatementSyntax, VariableDeclaratorSyntax, bool> variableUsedInReturnStatement)
        {
            if (twoFerSolution.NameMethod.Body.Statements.Count != 2)
                return false;

            var localDeclaration = twoFerSolution.NameMethod.Body.Statements[0] as LocalDeclarationStatementSyntax;
            var returnStatement = twoFerSolution.NameMethod.Body.Statements[1] as ReturnStatementSyntax;

            if (returnStatement == null ||
                localDeclaration == null || 
                localDeclaration.Declaration.Variables.Count != 1)
                return false;

            if (!localDeclaration.Declaration.Type.IsSafeEquivalentTo(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
                !localDeclaration.Declaration.Type.IsSafeEquivalentTo(IdentifierName("var")))
                return false;
            
            var variable = localDeclaration.Declaration.Variables[0];
            if (!variable.Initializer.IsSafeEquivalentTo(
                    EqualsValueClause(
                        CreateCoalesceExpression( // create twoFerSolution.InputParameter.ToCoalesceExpression()
                            IdentifierName(twoFerSolution.InputParameter.Identifier)))) &&
                !variable.Initializer.IsSafeEquivalentTo(
                    EqualsValueClause(
                        CreateConditionalExpression( // create twoFerSolution.InputParameter.ToConditionalExpression()
                            IdentifierName(twoFerSolution.InputParameter.Identifier)))))
            {
                return false;
            }

            return variableUsedInReturnStatement(returnStatement, variable);
        }

        private class TwoFerSolution : ParsedSolution
        {
            public ClassDeclarationSyntax TwoFerClass { get; }
            public MethodDeclarationSyntax NameMethod { get; }

            public ParameterSyntax InputParameter { get; }

            public TwoFerSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                TwoFerClass = solution.SyntaxRoot.GetClass("TwoFer");
                NameMethod = TwoFerClass.GetMethod("Name");
                InputParameter = NameMethod?.ParameterList.Parameters.FirstOrDefault();
            }
        }
    }
}