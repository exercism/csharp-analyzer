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

            return 
                twoFerSolution.AnalyzeSolutionWithError() ??
                twoFerSolution.AnalyzeSingleLineSolution() ??
                twoFerSolution.AnalyzeVariableAssignmentSolution() ??
                twoFerSolution.ReferToMentor();
        }

        private static SolutionAnalysis AnalyzeSolutionWithError(this TwoFerSolution twoFerSolution)
        {
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
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringConcat);

            if (twoFerSolution.AssignsToParameter())
                return twoFerSolution.DisapproveWithComment(DontAssignToParameter);

            return null;
        }

        private static SolutionAnalysis AnalyzeSingleLineSolution(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.NameMethod.SingleExpression())
                return null;

            if (twoFerSolution.NameMethod.IsExpressionBody() &&
                (twoFerSolution.ReturnedExpression.IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                 twoFerSolution.ReturnedExpression.IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter)))
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.ReturnedExpression.IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.ReturnedExpression.IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (twoFerSolution.ReturnedExpression.IsIsNullOrEmptyInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnedExpression.IsIsNullOrWhiteSpaceInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnedExpression.IsTernaryOperatorInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.UsesYouStringAsDefaultValue() &&
                twoFerSolution.ReturnedExpression.IsDefaultStringConcatenationExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.UsesNullAsDefaultValue() &&
                twoFerSolution.ReturnedExpression.IsNullCoalescingStringConcatenationExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.ReturnedExpression.IsTernaryOperatorStringConcatenationExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.ReturnedExpression.IsDefaultStringFormatExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.ReturnedExpression.IsTernaryOperatorStringFormatExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.ReturnedExpression.IsNullCoalescingStringFormatExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            return null;
        }

        private static SolutionAnalysis AnalyzeVariableAssignmentSolution(this TwoFerSolution twoFerSolution)
        {
            var variableDeclarator = twoFerSolution.NameMethod.AssignedVariable();
            if (variableDeclarator == null)
                return null;

            if (!twoFerSolution.VariableAssignedUsingNullCoalescingOperator() &&
                !twoFerSolution.VariableAssignedUsingTernaryOperator() &&
                !twoFerSolution.VariableAssignedUsingIsNullOrEmpty() &&
                !twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnedExpression.IsSafeEquivalentTo(
                    CreateStringFormatInvocationExpression(
                        IdentifierName(variableDeclarator.Identifier))))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnedExpression.IsSafeEquivalentTo(
                    CreateStringConcatenationExpression(
                        IdentifierName(variableDeclarator.Identifier))))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (!IsInterpolatedStringExpression(twoFerSolution.ReturnedExpression,
                    Interpolation(
                        IdentifierName(variableDeclarator.Identifier))))
                return null;

            if (twoFerSolution.VariableAssignedUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.VariableAssignedUsingTernaryOperator())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrEmpty())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace())
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }

        private static bool VariableAssignedUsingNullCoalescingOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsSafeEquivalentTo(
                EqualsValueClause(
                    CreateCoalesceExpression(
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

        private static bool VariableAssignedUsingTernaryOperator(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsSafeEquivalentTo(
                EqualsValueClause(
                    CreateConditionalExpression(
                        BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            IdentifierName(twoFerSolution.InputParameter.Identifier),
                            LiteralExpression(
                                SyntaxKind.NullLiteralExpression)), 
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

        private static bool VariableAssignedUsingIsNullOrEmpty(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsSafeEquivalentTo(
                EqualsValueClause(
                    CreateConditionalExpression(
                        CreateStringInvocationExpressionOnParameter("IsNullOrEmpty", twoFerSolution.InputParameter),
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

        private static bool VariableAssignedUsingIsNullOrWhiteSpace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.Variable.Initializer.IsSafeEquivalentTo(
                EqualsValueClause(
                    CreateConditionalExpression(
                        CreateStringInvocationExpressionOnParameter("IsNullOrWhiteSpace", twoFerSolution.InputParameter),
                        IdentifierName(twoFerSolution.InputParameter.Identifier))));

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
                        memberAccessExpression.Name.IsSafeEquivalentTo(IdentifierName("Replace")));

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
                CreateInterpolationWithConditionalExpression(
                    BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        IdentifierName(inputParameter.Identifier),
                        LiteralExpression(
                            SyntaxKind.NullLiteralExpression))));

        private static InterpolationSyntax CreateInterpolationWithConditionalExpression(ExpressionSyntax conditional) =>
            Interpolation(
                    ConditionalExpression(
                    conditional,
                    LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal("you")),
                    IdentifierName(
                        MissingToken(SyntaxKind.IdentifierToken)))
                .WithColonToken(
                    MissingToken(SyntaxKind.ColonToken).WithoutTrivia()))
                .WithFormatClause(
                    InterpolationFormatClause(
                            Token(SyntaxKind.ColonToken))
                        .WithFormatStringToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.InterpolatedStringTextToken,
                                " input",
                                " input",
                                TriviaList())));

        private static bool IsNullCoalescingInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression.IsInterpolatedStringExpression(
                Interpolation(
                    CreateCoalesceExpression(
                        IdentifierName(parameter.Identifier))));

        private static bool IsIsNullOrEmptyInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression.IsInterpolatedStringExpression(
                CreateInterpolationWithConditionalExpression(
                    CreateStringInvocationExpressionOnParameter("IsNullOrEmpty", parameter)));

        private static bool IsIsNullOrWhiteSpaceInterpolatedStringExpression(this ExpressionSyntax expression, ParameterSyntax parameter) =>
            expression.IsInterpolatedStringExpression(
                CreateInterpolationWithConditionalExpression(
                    CreateStringInvocationExpressionOnParameter("IsNullOrWhiteSpace", parameter)));

        private static InvocationExpressionSyntax CreateStringInvocationExpressionOnParameter(string methodName, ParameterSyntax parameter) =>
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
            var interpolatedStringExpressionSyntax = CreateInterpolatedStringExpression(interpolation);
            var syntaxNode = triviaRemover.Visit(expression);
            var visit = triviaRemover.Visit(interpolatedStringExpressionSyntax);


            return syntaxNode.ToFullString() == visit.ToFullString();
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

        private static ConditionalExpressionSyntax CreateConditionalExpression(ExpressionSyntax condition, IdentifierNameSyntax identifierName) =>
            ConditionalExpression(
                condition,
                LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    Literal("you")),
                identifierName);

        private static bool IsDefaultStringConcatenationExpression(this ExpressionSyntax expression, ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringConcatenationExpression(
                    IdentifierName(inputParameter.Identifier)));

        private static bool IsNullCoalescingStringConcatenationExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringConcatenationExpression(
                    ParenthesizedExpression(
                        CreateCoalesceExpression(
                            IdentifierName(inputParameter.Identifier)))));

        private static bool IsTernaryOperatorStringConcatenationExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringConcatenationExpression(
                    ParenthesizedExpression(
                        CreateTernaryOperatorConditionalExpression(inputParameter))));

        private static ConditionalExpressionSyntax CreateTernaryOperatorConditionalExpression(ParameterSyntax inputParameter) =>
            CreateConditionalExpression(
                BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    IdentifierName(inputParameter.Identifier),
                    LiteralExpression(
                        SyntaxKind.NullLiteralExpression)),
                IdentifierName(inputParameter.Identifier));

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

        private static bool IsDefaultStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringFormatInvocationExpression(
                    IdentifierName(inputParameter.Identifier)));

        private static bool IsTernaryOperatorStringFormatExpression(this ExpressionSyntax expression,
            ParameterSyntax inputParameter) =>
            expression.IsSafeEquivalentTo(
                CreateStringFormatInvocationExpression(
                    CreateTernaryOperatorConditionalExpression(inputParameter))) ||
            expression.IsSafeEquivalentTo(
                CreateStringFormatInvocationExpression(
                    CreateConditionalExpression(
                        CreateStringInvocationExpressionOnParameter("IsNullOrEmpty", inputParameter),
                        IdentifierName(inputParameter.Identifier)))) ||
            expression.IsSafeEquivalentTo(
                CreateStringFormatInvocationExpression(
                    CreateConditionalExpression(
                        CreateStringInvocationExpressionOnParameter("IsNullOrWhiteSpace", inputParameter),
                        IdentifierName(inputParameter.Identifier))));

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

        private static VariableDeclaratorSyntax AssignedVariable(this MethodDeclarationSyntax nameMethod)
        {
            if (nameMethod.Body == null ||
                nameMethod.Body.Statements.Count != 2)
                return null;

            if (!(nameMethod.Body.Statements[1] is ReturnStatementSyntax) ||
                !(nameMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration))
                return null;
            
            if (localDeclaration.Declaration.Variables.Count != 1 ||
                !localDeclaration.Declaration.Type.IsSafeEquivalentTo(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
                !localDeclaration.Declaration.Type.IsSafeEquivalentTo(IdentifierName("var")))
                return null;

            return localDeclaration.Declaration.Variables[0];
        }

        private class TwoFerSolution : ParsedSolution
        {
            public ClassDeclarationSyntax TwoFerClass { get; }
            public MethodDeclarationSyntax NameMethod { get; }
            public ParameterSyntax InputParameter { get; }
            public ExpressionSyntax ReturnedExpression { get; }
            public VariableDeclaratorSyntax Variable { get; }

            public TwoFerSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                TwoFerClass = solution.SyntaxRoot.GetClass("TwoFer");
                NameMethod = TwoFerClass.GetMethod("Name");
                InputParameter = NameMethod?.ParameterList.Parameters.FirstOrDefault();
                ReturnedExpression = NameMethod?.ReturnedExpression();
                Variable = NameMethod?.AssignedVariable();
            }
        }
    }
}