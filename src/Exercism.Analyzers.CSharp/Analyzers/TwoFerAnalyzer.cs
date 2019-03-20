using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFerSolutions;
using static Exercism.Analyzers.CSharp.Analyzers.SharedComments;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFerComments;

namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class TwoFerAnalyzer
    {
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            if (parsedSolution.UsesOverloads() ||
                parsedSolution.UsesDuplicateString())
                return parsedSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);
            
            if (parsedSolution.UsesStringReplace())
                return parsedSolution.DisapproveWithComment(UseStringInterpolationNotStringReplace);
            
            if (parsedSolution.AssignsToParameter())
                return parsedSolution.DisapproveWithComment(DontAssignToParameter);
            
            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return parsedSolution.ApproveAsOptimal();

            if (parsedSolution.IsEquivalentTo(StringInterpolationWithTernaryOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithTernaryOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody) ||
                parsedSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (parsedSolution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody) ||
                parsedSolution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody) ||
                parsedSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody) ||
                parsedSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return parsedSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            return parsedSolution.ReferToMentor();
        }

        private static bool UsesOverloads(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot
                .GetClass("TwoFer")
                .GetMethods("Name")
                .Count() > 1;

        private static bool UsesDuplicateString(this ParsedSolution parsedSolution)
        {
            var nameMethod = parsedSolution.GetNameMethod();

            if (nameMethod == null)
                return false;

            var literalExpressionCount = nameMethod
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("one for me"));

            var interpolatedStringTextCount = nameMethod
                .DescendantNodes()
                .OfType<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("one for me"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        private static bool UsesStringReplace(this ParsedSolution parsedSolution)
        {
            var nameMethod = parsedSolution.GetNameMethod();

            var replaceExpression = nameMethod
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .FirstOrDefault(memberAccessExpression => memberAccessExpression.Name.Identifier.ValueText == "Replace");

            var invocationExpression = replaceExpression?.Parent as InvocationExpressionSyntax;
            if (invocationExpression == null || invocationExpression.ArgumentList.Arguments.Count != 2)
                return false;

            var secondArgumentIdentifierName = invocationExpression.ArgumentList.Arguments[1].Expression as IdentifierNameSyntax;
            return secondArgumentIdentifierName?.Identifier.ValueText == "input";
        }

        private static bool AssignsToParameter(this ParsedSolution parsedSolution) =>
            parsedSolution
                .GetNameMethod()
                .AssignsToIdentifier("input");

        private static MethodDeclarationSyntax GetNameMethod(this ParsedSolution parsedSolution) =>
            parsedSolution.SyntaxRoot
                .GetClass("TwoFer")
                .GetMethod("Name");
    }
}