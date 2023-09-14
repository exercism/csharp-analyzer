namespace Exercism.Analyzers.CSharp.Exercises;

internal class TwoFerAnalyzer : ExerciseAnalyzer
{
    protected override void AnalyzeExerciseSpecific(Solution solution)
    {
        // TODO
    }
}

// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
//
// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
// using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;
//
// internal static class TwoFerSyntaxFactory
// {
//     public static ExpressionStatementSyntax TwoFerAssignParameterStatement(ExpressionSyntax condition, IdentifierNameSyntax parameterName) =>
//         ExpressionStatement(
//             SimpleAssignmentExpression(
//                 parameterName,
//                 condition));
//
//     public static IfStatementSyntax TwoFerAssignParameterIfStatement(ExpressionSyntax condition, IdentifierNameSyntax parameterName) =>
//         IfStatement(
//             condition,
//             Block(
//                 SingletonList<StatementSyntax>(
//                     ExpressionStatement(
//                         SimpleAssignmentExpression(
//                             parameterName,
//                             StringLiteralExpression("you"))))));
//
//     public static BinaryExpressionSyntax TwoFerParameterIsNullExpression(TwoFerSolution solution) =>
//         EqualsExpression(
//             TwoFerParameterIdentifierName(solution),
//             NullLiteralExpression());
//
//     public static IdentifierNameSyntax TwoFerParameterIdentifierName(TwoFerSolution solution) =>
//         IdentifierName(solution.SpeakMethodParameterName);
//
//     public static IdentifierNameSyntax TwoFerVariableIdentifierName(TwoFerSolution solution) =>
//         IdentifierName(solution.TwoFerVariableName);
//
//     public static BinaryExpressionSyntax TwoFerCoalesceExpression(IdentifierNameSyntax identifierName) =>
//         CoalesceExpression(
//             identifierName,
//             StringLiteralExpression("you"));
//
//     public static InterpolatedStringExpressionSyntax TwoFerInterpolatedStringExpression(ExpressionSyntax interpolationExpression) =>
//         InterpolatedStringExpression(
//                 Token(SyntaxKind.InterpolatedStringStartToken))
//             .WithContents(
//                 List(new InterpolatedStringContentSyntax[]
//                 {
//                     InterpolatedStringText("One for "),
//                     Interpolation(interpolationExpression),
//                     InterpolatedStringText(", one for me.")
//                 }));
//
//     public static ConditionalExpressionSyntax TwoFerConditionalExpression(ExpressionSyntax condition, IdentifierNameSyntax identifierName) =>
//         ConditionalExpression(
//             condition,
//             StringLiteralExpression("you"),
//             identifierName);
//
//     public static ConditionalExpressionSyntax TwoFerConditionalExpressionWithNullCheck(TwoFerSolution solution) =>
//         TwoFerConditionalExpression(
//             EqualsExpression(
//                 IdentifierName(solution.SpeakMethodParameterName),
//                 NullLiteralExpression()),
//             IdentifierName(solution.SpeakMethodParameterName));
//
//     public static BinaryExpressionSyntax TwoFerStringConcatenationExpression(ExpressionSyntax nameExpression) =>
//         AddExpression(
//             AddExpression(
//                 StringLiteralExpression("One for "),
//                 nameExpression),
//             StringLiteralExpression(", one for me."));
//
//     public static InvocationExpressionSyntax TwoFerStringFormatInvocationExpression(ExpressionSyntax argumentExpression) =>
//         InvocationExpression(
//                 StringMemberAccessExpression(
//                     IdentifierName("Format")))
//             .WithArgumentList(
//                 ArgumentList(
//                     SeparatedList<ArgumentSyntax>(
//                         new SyntaxNodeOrToken[]{
//                             Argument(
//                                 StringLiteralExpression("One for {0}, one for me.")),
//                             Token(SyntaxKind.CommaToken),
//                             Argument(argumentExpression)})));
//
//     public static InvocationExpressionSyntax TwoFerIsNullOrEmptyInvocationExpression(TwoFerSolution twoFerSolution) =>
//         TwoFerStringInvocationExpression(twoFerSolution, IdentifierName("IsNullOrEmpty"));
//
//     public static InvocationExpressionSyntax TwoFerIsNullOrWhiteSpaceInvocationExpression(TwoFerSolution twoFerSolution) =>
//         TwoFerStringInvocationExpression(twoFerSolution, IdentifierName("IsNullOrWhiteSpace"));
//
//     private static InvocationExpressionSyntax TwoFerStringInvocationExpression(TwoFerSolution twoFerSolution, IdentifierNameSyntax stringMethodIdentifierName) =>
//         StringInvocationExpression(
//             stringMethodIdentifierName,
//             TwoFerParameterIdentifierName(twoFerSolution));
//
//     public static InterpolatedStringExpressionSyntax TwoFerConditionalInterpolatedStringExpression(ExpressionSyntax condition, IdentifierNameSyntax identifierName) =>
//         TwoFerInterpolatedStringExpression(
//             ParenthesizedExpression(
//                 TwoFerConditionalExpression(condition, identifierName)));
//
//     public static ConditionalExpressionSyntax TwoFerParameterIsNullConditionalExpression(TwoFerSolution solution) =>
//         TwoFerConditionalExpression(
//             TwoFerParameterIsNullExpression(solution),
//             TwoFerParameterIdentifierName(solution));
//
//     public static ConditionalExpressionSyntax TwoFerParameterIsNullOrEmptyConditionalExpression(TwoFerSolution solution) =>
//         TwoFerConditionalExpression(
//             TwoFerIsNullOrEmptyInvocationExpression(solution),
//             TwoFerParameterIdentifierName(solution));
//
//     public static ConditionalExpressionSyntax TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(TwoFerSolution solution) =>
//         TwoFerConditionalExpression(
//             TwoFerIsNullOrWhiteSpaceInvocationExpression(solution),
//             TwoFerParameterIdentifierName(solution));
// }
//
// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;
// using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerComments;
// using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSolution;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;
//
// internal class TwoFerAnalyzer : ExerciseAnalyzer<TwoFerSolution>
// {
//     protected override SolutionAnalysis AnalyzeSpecific(TwoFerSolution solution)
//     {
//         if (solution.MissingTwoFerClass)
//             return AnalysisWithComment(MissingClass(TwoFerClassName));
//
//         if (solution.MissingSpeakMethod)
//             return AnalysisWithComment(MissingMethod(SpeakMethodName));
//
//         if (solution.InvalidSpeakMethod)
//             return AnalysisWithComment(InvalidMethodSignature(SpeakMethodName, SpeakMethodSignature));
//
//         if (solution.UsesOverloads)
//             return AnalysisWithComment(UseDefaultValueNotOverloads);
//
//         if (solution.UsesDuplicateString)
//             AddComment(UseSingleFormattedStringNotMultiple);
//
//         if (solution.NoDefaultValue)
//             AddComment(UseDefaultValue(solution.SpeakMethodParameterName));
//
//         if (solution.UsesInvalidDefaultValue)
//             AddComment(InvalidDefaultValue(solution.SpeakMethodParameterName, solution.SpeakMethodParameterDefaultValue));
//
//         if (solution.UsesStringReplace)
//             AddComment(UseStringInterpolationNotStringReplace);
//
//         if (solution.UsesStringJoin)
//             AddComment(UseStringInterpolationNotStringJoin);
//
//         if (solution.UsesStringConcat)
//             AddComment(UseStringInterpolationNotStringConcat);
//
//         if (solution.UsesStringConcatenation)
//             AddComment(UseStringInterpolationNotStringConcatenation);
//
//         if (solution.UsesStringFormat)
//             AddComment(UseStringInterpolationNotStringFormat);
//
//         if (solution.UsesIsNullOrEmptyCheck)
//             AddComment(UseNullCoalescingOperatorNotIsNullOrEmptyCheck);
//
//         if (solution.UsesIsNullOrWhiteSpaceCheck)
//             AddComment(UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck);
//
//         if (solution.UsesNullCheck)
//             AddComment(UseNullCoalescingOperatorNotNullCheck);
//
//         if (solution.CanUseExpressionBody)
//             AddComment(UseExpressionBodiedMember(SpeakMethodName));
//         
//         if (!solution.AssignsParameterUsingKnownExpression)
//             return Analysis;
//
//         if (solution.AssignsParameterUsingNullCoalescingOperator)
//             AddComment(DoNotAssignAndReturn);
//
//         return Analysis;
//     }
// }
//
// using System.Linq;
//
// using Exercism.Analyzers.CSharp.Syntax;
// using Exercism.Analyzers.CSharp.Syntax.Comparison;
//
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
//
// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
// using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntaxFactory;
// using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;
//
// internal class TwoFerSolution : Solution
// {
//     public const string TwoFerClassName = "TwoFer";
//     public const string SpeakMethodName = "Speak";
//     public const string SpeakMethodSignature = "public static string Speak(string name)";
//
//     public TwoFerSolution(Solution solution) : base(solution)
//     {
//     }
//
//     private ClassDeclarationSyntax TwoFerClass =>
//         SyntaxRoot.GetClass(TwoFerClassName);
//
//     private MethodDeclarationSyntax SpeakMethod =>
//         TwoFerClass?.GetMethod(SpeakMethodName);
//
//     private ParameterSyntax SpeakMethodParameter =>
//         SpeakMethod?.ParameterList.Parameters.FirstOrDefault();
//
//     private ExpressionSyntax TwoFerExpression =>
//         SpeakMethod?.ReturnedExpression();
//
//     private VariableDeclaratorSyntax TwoFerVariable
//     {
//         get
//         {
//             var speakMethod = SpeakMethod;
//             if (speakMethod == null ||
//                 speakMethod.Body == null ||
//                 speakMethod.Body.Statements.Count != 2)
//                 return null;
//
//             if (!(speakMethod.Body.Statements[1] is ReturnStatementSyntax) ||
//                 !(speakMethod.Body.Statements[0] is LocalDeclarationStatementSyntax localDeclaration))
//                 return null;
//
//             if (localDeclaration.Declaration.Variables.Count != 1 ||
//                 !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(PredefinedType(Token(SyntaxKind.StringKeyword))) &&
//                 !localDeclaration.Declaration.Type.IsEquivalentWhenNormalized(IdentifierName("var")))
//                 return null;
//
//             return localDeclaration.Declaration.Variables[0];
//         }
//     }
//
//     public string SpeakMethodParameterName =>
//         SpeakMethodParameter.Identifier.Text;
//
//     public string SpeakMethodParameterDefaultValue =>
//         SpeakMethodParameter.Default.Value.ToFullString();
//
//     public string TwoFerVariableName =>
//         TwoFerVariable.Identifier.Text;
//
//     public bool AssignsToParameter =>
//         SpeakMethodParameter != null &&
//         SpeakMethod.AssignsToParameter(SpeakMethodParameter);
//
//     public bool CanUseExpressionBody =>
//         SpeakMethod.CanUseExpressionBody();
//
//     public bool UsesExpressionBody =>
//         SpeakMethod.IsExpressionBody();
//
//     public bool UsesStringConcatenation =>
//         ReturnsStringConcatenationWithDefaultValue ||
//         ReturnsStringConcatenationWithNullCoalescingOperator ||
//         ReturnsStringConcatenationWithTernaryOperator ||
//         ReturnsStringConcatenationWithVariable;
//
//     private bool ReturnsStringConcatenationWithDefaultValue =>
//         Returns(
//             TwoFerStringConcatenationExpression(
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool ReturnsStringConcatenationWithNullCoalescingOperator =>
//         Returns(
//             TwoFerStringConcatenationExpression(
//                 ParenthesizedExpression(
//                     TwoFerCoalesceExpression(
//                         TwoFerParameterIdentifierName(this)))));
//
//     private bool ReturnsStringConcatenationWithTernaryOperator =>
//         ReturnsStringConcatenationWithNullCheck ||
//         ReturnsStringConcatenationWithIsNullOrEmptyCheck ||
//         ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck;
//
//     private bool ReturnsStringConcatenationWithIsNullOrWhiteSpaceCheck =>
//         Returns(
//             TwoFerStringConcatenationExpression(
//                 ParenthesizedExpression(
//                     TwoFerConditionalExpression(
//                         TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
//                         TwoFerParameterIdentifierName(this)))));
//
//     private bool ReturnsStringConcatenationWithIsNullOrEmptyCheck =>
//         Returns(
//             TwoFerStringConcatenationExpression(
//                 ParenthesizedExpression(
//                     TwoFerConditionalExpression(
//                         TwoFerIsNullOrEmptyInvocationExpression(this),
//                         TwoFerParameterIdentifierName(this)))));
//
//     private bool ReturnsStringConcatenationWithNullCheck =>
//         Returns(
//             TwoFerStringConcatenationExpression(
//                 ParenthesizedExpression(
//                     TwoFerConditionalExpressionWithNullCheck(this))));
//
//     public bool UsesStringFormat =>
//         ReturnsStringFormatWithDefaultValue ||
//         ReturnsStringFormatWithNullCoalescingOperator ||
//         ReturnsStringFormatWithTernaryOperator ||
//         ReturnsStringFormatWithVariable;
//
//     private bool ReturnsStringFormatWithDefaultValue =>
//         Returns(
//             TwoFerStringFormatInvocationExpression(
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool ReturnsStringFormatWithNullCoalescingOperator =>
//         Returns(
//             TwoFerStringFormatInvocationExpression(
//                 TwoFerCoalesceExpression(
//                     TwoFerParameterIdentifierName(this))));
//
//     private bool ReturnsStringFormatWithTernaryOperator =>
//         ReturnsStringFormatWithNullCheck ||
//         ReturnsStringFormatWithIsNullOrEmptyCheck ||
//         ReturnsStringFormatWithIsNullOrWhiteSpaceCheck;
//
//     private bool ReturnsStringFormatWithIsNullOrWhiteSpaceCheck =>
//         Returns(
//             TwoFerStringFormatInvocationExpression(
//                 TwoFerConditionalExpression(
//                     TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
//                     TwoFerParameterIdentifierName(this))));
//
//     private bool ReturnsStringFormatWithIsNullOrEmptyCheck =>
//         Returns(
//             TwoFerStringFormatInvocationExpression(
//                 TwoFerConditionalExpression(
//                     TwoFerIsNullOrEmptyInvocationExpression(this),
//                     TwoFerParameterIdentifierName(this))));
//
//     private bool ReturnsStringFormatWithNullCheck =>
//         Returns(
//             TwoFerStringFormatInvocationExpression(
//                 TwoFerConditionalExpressionWithNullCheck(this)));
//
//     public bool ReturnsStringInterpolation =>
//         UsesStringInterpolationWithDefaultValue ||
//         UsesStringInterpolationWithNullCheck ||
//         UsesStringInterpolationWithNullCoalescingOperator ||
//         UsesStringInterpolationWithIsNullOrEmptyCheck ||
//         UsesStringInterpolationWithIsNullOrWhiteSpaceCheck;
//
//     public bool UsesStringInterpolationWithDefaultValue =>
//         Returns(
//             TwoFerInterpolatedStringExpression(
//                 TwoFerParameterIdentifierName(this)));
//
//     public bool UsesStringInterpolationWithNullCheck =>
//         Returns(
//             TwoFerConditionalInterpolatedStringExpression(
//                 EqualsExpression(
//                     TwoFerParameterIdentifierName(this),
//                     NullLiteralExpression()),
//                 TwoFerParameterIdentifierName(this)));
//
//     public bool UsesStringInterpolationWithNullCoalescingOperator =>
//         Returns(
//             TwoFerInterpolatedStringExpression(
//                 TwoFerCoalesceExpression(
//                     TwoFerParameterIdentifierName(this))));
//
//     private bool UsesStringInterpolationWithIsNullOrEmptyCheck =>
//         Returns(
//             TwoFerConditionalInterpolatedStringExpression(
//                 TwoFerIsNullOrEmptyInvocationExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool UsesStringInterpolationWithIsNullOrWhiteSpaceCheck =>
//         Returns(
//             TwoFerConditionalInterpolatedStringExpression(
//                 TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     public bool AssignsParameterUsingKnownExpression =>
//         AssignsParameterUsingNullCoalescingOperator ||
//         AssignsParameterUsingNullCheck ||
//         AssignsParameterUsingIfNullCheck ||
//         AssignsParameterUsingIsNullOrEmptyCheck ||
//         AssignsParameterUsingIfIsNullOrEmptyCheck ||
//         AssignsParameterUsingIsNullOrWhiteSpaceCheck ||
//         AssignsParameterUsingIfIsNullOrWhiteSpaceCheck;
//
//     public bool AssignsParameterUsingNullCoalescingOperator =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterStatement(
//                 TwoFerCoalesceExpression(
//                     TwoFerParameterIdentifierName(this)),
//                 TwoFerParameterIdentifierName(this)));
//
//     public bool AssignsParameterUsingNullCheck =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterStatement(
//                 TwoFerParameterIsNullConditionalExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     public bool AssignsParameterUsingIfNullCheck =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterIfStatement(
//                 TwoFerParameterIsNullExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool AssignsParameterUsingIsNullOrEmptyCheck =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterStatement(
//                 TwoFerParameterIsNullOrEmptyConditionalExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool AssignsParameterUsingIfIsNullOrEmptyCheck =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterIfStatement(
//                 TwoFerIsNullOrEmptyInvocationExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool AssignsParameterUsingIsNullOrWhiteSpaceCheck =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterStatement(
//                 TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool AssignsParameterUsingIfIsNullOrWhiteSpaceCheck =>
//         ParameterAssignedUsingStatement(
//             TwoFerAssignParameterIfStatement(
//                 TwoFerIsNullOrWhiteSpaceInvocationExpression(this),
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool ParameterAssignedUsingStatement(SyntaxNode statement) =>
//         AssignsStatement &&
//         AssignmentStatement.IsEquivalentWhenNormalized(statement);
//
//     private bool AssignsStatement =>
//         AssignmentStatement != null;
//
//     private StatementSyntax AssignmentStatement =>
//         SpeakMethod.Body?.Statements[0];
//
//     public bool AssignsVariable =>
//         TwoFerVariable != null;
//
//     public bool AssignsVariableUsingKnownInitializer =>
//         AssignsVariableUsingNullCoalescingOperator ||
//         AssignsVariableUsingNullCheck ||
//         AssignsVariableUsingIsNullOrEmptyCheck ||
//         AssignsVariableUsingIsNullOrWhiteSpaceCheck;
//
//     public bool AssignsVariableUsingNullCoalescingOperator =>
//         AssignsVariableUsingExpression(
//             TwoFerCoalesceExpression(
//                 TwoFerParameterIdentifierName(this)));
//
//     private bool AssignsVariableUsingNullCheck =>
//         AssignsVariableUsingExpression(TwoFerParameterIsNullConditionalExpression(this));
//
//     private bool AssignsVariableUsingIsNullOrEmptyCheck =>
//         AssignsVariableUsingExpression(TwoFerParameterIsNullOrEmptyConditionalExpression(this));
//
//     private bool AssignsVariableUsingIsNullOrWhiteSpaceCheck =>
//         AssignsVariableUsingExpression(TwoFerParameterIsNullOrWhiteSpaceConditionalExpression(this));
//
//     private bool AssignsVariableUsingExpression(ExpressionSyntax initializer) =>
//         AssignsVariable &&
//         TwoFerVariable.Initializer.IsEquivalentWhenNormalized(
//             EqualsValueClause(initializer));
//
//     public bool ReturnsStringInterpolationWithVariable =>
//         AssignsVariable &&
//         Returns(
//             TwoFerInterpolatedStringExpression(
//                 TwoFerVariableIdentifierName(this)));
//
//     private bool ReturnsStringConcatenationWithVariable =>
//         AssignsVariable &&
//         Returns(
//             TwoFerStringConcatenationExpression(
//                 TwoFerVariableIdentifierName(this)));
//
//     private bool ReturnsStringFormatWithVariable =>
//         AssignsVariable &&
//         Returns(
//             TwoFerStringFormatInvocationExpression(
//                 TwoFerVariableIdentifierName(this)));
//
//     private bool Returns(SyntaxNode returned) =>
//         TwoFerExpression.IsEquivalentWhenNormalized(returned);
//
//     public bool MissingTwoFerClass =>
//         TwoFerClass == null;
//
//     public bool MissingSpeakMethod =>
//         !MissingTwoFerClass & SpeakMethod == null;
//
//     public bool InvalidSpeakMethod =>
//         SpeakMethod != null &&
//         !TwoFerClass.GetMethods(SpeakMethodName).Any(
//             speakMethod =>
//                 speakMethod.ParameterList.Parameters.Count == 1 &&
//                 speakMethod.ParameterList.Parameters[0].Type.IsEquivalentWhenNormalized(
//                     PredefinedType(Token(SyntaxKind.StringKeyword))) &&
//                 speakMethod.ReturnType.IsEquivalentWhenNormalized(
//                     PredefinedType(Token(SyntaxKind.StringKeyword))));
//
//     public bool UsesOverloads =>
//         TwoFerClass.GetMethods(SpeakMethodName).Count() > 1;
//
//     public bool UsesDuplicateString
//     {
//         get
//         {
//             var literalExpressionCount = SpeakMethod
//                 .DescendantNodes<LiteralExpressionSyntax>()
//                 .Count(literalExpression => literalExpression.Token.ValueText.Contains("One for"));
//
//             var interpolatedStringTextCount = SpeakMethod
//                 .DescendantNodes<InterpolatedStringTextSyntax>()
//                 .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("One for"));
//
//             return literalExpressionCount + interpolatedStringTextCount > 1;
//         }
//     }
//
//     public bool UsesStringJoin =>
//         SpeakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Join")));
//
//     public bool UsesStringConcat =>
//         SpeakMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Concat")));
//
//     public bool UsesStringReplace =>
//         SpeakMethod.InvokesMethod(IdentifierName("Replace"));
//
//     public bool NoDefaultValue =>
//         SpeakMethodParameter != null &&
//         SpeakMethod?.ParameterList != null &&
//         SpeakMethod.ParameterList.Parameters.All(parameter => parameter.Default == null);
//
//     public bool UsesInvalidDefaultValue =>
//         UsesDefaultValue &&
//         !DefaultValueIsNull &&
//         !DefaultValueIsYouString &&
//         !DefaultValueIsYouStringSpecifiedAsConst;
//
//     private bool UsesDefaultValue =>
//         SpeakMethodParameter?.Default != null;
//
//     private bool DefaultValueIsNull =>
//         SpeakMethodParameter.Default.Value.IsEquivalentWhenNormalized(NullLiteralExpression());
//
//     private bool DefaultValueIsYouString =>
//         SpeakMethodParameter.Default.Value.IsEquivalentWhenNormalized(StringLiteralExpression("you"));
//
//     private bool DefaultValueIsYouStringSpecifiedAsConst =>
//         SpeakMethodParameter.Default.Value is IdentifierNameSyntax identifierName &&
//         TwoFerClass.AssignedVariableWithName(identifierName).IsEquivalentWhenNormalized(
//             SyntaxFactory.VariableDeclarator(identifierName.Identifier, default, EqualsValueClause(StringLiteralExpression("you"))));
//
//     public bool UsesIsNullOrEmptyCheck =>
//         UsesStringInterpolationWithIsNullOrEmptyCheck ||
//         AssignsParameterUsingIsNullOrEmptyCheck ||
//         AssignsParameterUsingIfIsNullOrEmptyCheck ||
//         AssignsVariableUsingIsNullOrEmptyCheck;
//
//     public bool UsesIsNullOrWhiteSpaceCheck =>
//         UsesStringInterpolationWithIsNullOrWhiteSpaceCheck ||
//         AssignsVariableUsingIsNullOrWhiteSpaceCheck ||
//         AssignsParameterUsingIsNullOrWhiteSpaceCheck ||
//         AssignsParameterUsingIfIsNullOrWhiteSpaceCheck;
//
//     public bool UsesNullCheck =>
//         UsesStringInterpolationWithNullCheck ||
//         AssignsParameterUsingNullCheck ||
//         AssignsParameterUsingIfNullCheck ||
//         AssignsVariableUsingNullCheck;
// }
//
// using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedCommentParameters;
//
// namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer;
//
// internal static class TwoFerComments
// {
//     public static readonly SolutionComment UseSingleFormattedStringNotMultiple = new("csharp.two-fer.use_single_formatted_string_not_multiple", SolutionCommentType.Essential);
//     public static readonly SolutionComment UseStringInterpolationNotStringReplace = new("csharp.two-fer.use_string_interpolation_not_string_replace", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseStringInterpolationNotStringJoin = new("csharp.two-fer.use_string_interpolation_not_string_join", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseStringInterpolationNotStringConcat = new("csharp.two-fer.use_string_interpolation_not_string_concat", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseNullCoalescingOperatorNotIsNullOrEmptyCheck = new("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_empty", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseNullCoalescingOperatorNotIsNullOrWhiteSpaceCheck = new("csharp.two-fer.use_null_coalescing_operator_not_is_null_or_white_space", SolutionCommentType.Actionable);
//     public static readonly SolutionComment UseDefaultValueNotOverloads = new("csharp.two-fer.use_default_value_not_overloads", SolutionCommentType.Essential);
//
//     public static SolutionComment UseDefaultValue(string parameterName) =>
//         new("csharp.two-fer.use_default_value", SolutionCommentType.Essential, new SolutionCommentParameter(Name, parameterName));
//
//     public static SolutionComment InvalidDefaultValue(string parameterName, string defaultValue) =>
//         new("csharp.two-fer.invalid_default_value", SolutionCommentType.Actionable, new SolutionCommentParameter(Name, parameterName), new SolutionCommentParameter(Value, defaultValue));
// }