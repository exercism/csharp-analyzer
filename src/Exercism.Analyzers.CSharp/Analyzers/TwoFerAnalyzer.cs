using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
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
            var twoFerSolution = new TwoFerSolution(parsedSolution);

            if (twoFerSolution.MissingNameMethod())
                return twoFerSolution.DisapproveWithComment(FixCompileErrors);
            
            if (twoFerSolution.UsesOverloads() ||
                twoFerSolution.UsesDuplicateString())
                return twoFerSolution.DisapproveWithComment(UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.UsesStringReplace())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin())
                return parsedSolution.DisapproveWithComment(UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(UseStringInterpolationNotStringConcat);

            if (twoFerSolution.AssignsToParameter())
                return twoFerSolution.DisapproveWithComment(DontAssignToParameter);

            if (twoFerSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(StringInterpolationWithNullCoalescingOperatorAndVariableForName))
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.IsEquivalentTo(StringInterpolationWithTernaryOperatorInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(StringInterpolationWithTernaryOperatorInBlockBody))
                return twoFerSolution.ApproveWithComment(UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.IsEquivalentTo(DefaultValueWithStringInterpolationInBlockBody) ||
                twoFerSolution.IsEquivalentTo(StringInterpolationWithInlinedNullCoalescingOperatorInBlockBody))
                return twoFerSolution.ApproveWithComment(UseExpressionBodiedMember);

            if (twoFerSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(DefaultValueWithStringConcatenationInBlockBody) ||
                twoFerSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(StringConcatenationWithInlinedNullCoalescingOperatorInBlockBody))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.IsEquivalentTo(DefaultValueWithStringFormatInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(DefaultValueWithStringFormatInBlockBody) ||
                twoFerSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInExpressionBody) ||
                twoFerSolution.IsEquivalentTo(StringFormatWithInlinedNullCoalescingOperatorInBlockBody))
                return twoFerSolution.ApproveWithComment(UseStringInterpolationNotStringFormat);

            return twoFerSolution.ReferToMentor();
        }

        private static bool MissingNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod == null;

        private static bool UsesOverloads(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.TwoFerClass.GetMethods("Name").Count() > 1;

        private static bool UsesDuplicateString(this TwoFerSolution twoFerSolution)
        {
            var literalExpressionCount = twoFerSolution.NameMethod
                .DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .Count(literalExpression => literalExpression.Token.ValueText.Contains("one for me"));

            var interpolatedStringTextCount = twoFerSolution.NameMethod
                .DescendantNodes()
                .OfType<InterpolatedStringTextSyntax>()
                .Count(interpolatedStringText => interpolatedStringText.TextToken.ValueText.Contains("one for me"));

            return literalExpressionCount + interpolatedStringTextCount > 1;
        }

        private static bool UsesStringReplace(this TwoFerSolution twoFerSolution)
        {
            var replaceExpression = twoFerSolution.NameMethod
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .FirstOrDefault(memberAccessExpression => memberAccessExpression.Name.Identifier.ValueText == "Replace");

            var invocationExpression = replaceExpression?.Parent as InvocationExpressionSyntax;
            if (invocationExpression == null || invocationExpression.ArgumentList.Arguments.Count != 2)
                return false;

            var secondArgumentIdentifierName = invocationExpression.ArgumentList.Arguments[1].Expression as IdentifierNameSyntax;
            return secondArgumentIdentifierName?.Identifier.ValueText == "input";
        }

        private static bool UsesStringJoin(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod("System","string","Join") ||
            twoFerSolution.NameMethod.InvokesMethod("System","String","Join");

        private static bool UsesStringConcat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod("System","string","Concat") ||
            twoFerSolution.NameMethod.InvokesMethod("System","String","Concat");

        private static bool AssignsToParameter(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.AssignsToParameter("input") ||
            twoFerSolution.NameMethod.AssignsToParameter("name");

        private class TwoFerSolution : ParsedSolution
        {
            public ClassDeclarationSyntax TwoFerClass { get; }
            public MethodDeclarationSyntax NameMethod { get; }

            public TwoFerSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
            {
                TwoFerClass = solution.SyntaxRoot.GetClass("TwoFer");
                NameMethod = TwoFerClass.GetMethod("Name");
            }
        }
    }
}