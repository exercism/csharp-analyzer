using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerErrorAnalyzer
    {
        public static SolutionAnalysis AnalyzeError(this TwoFerSolution twoFerSolution)
        {
            if (twoFerSolution.UsesOverloads())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.MissingNameMethod() ||
                twoFerSolution.InvalidNameMethod())
                return twoFerSolution.DisapproveWithComment(SharedComments.FixCompileErrors);

            if (twoFerSolution.UsesDuplicateString())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseSingleFormattedStringNotMultiple);

            if (twoFerSolution.NoDefaultValue())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseDefaultValue);

            if (twoFerSolution.UsesInvalidDefaultValue())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.InvalidDefaultValue);

            if (twoFerSolution.UsesStringReplace())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseStringInterpolationNotStringReplace);

            if (twoFerSolution.UsesStringJoin())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseStringInterpolationNotStringJoin);

            if (twoFerSolution.UsesStringConcat())
                return twoFerSolution.DisapproveWithComment(TwoFerComments.UseStringInterpolationNotStringConcat);

            return null;
        }

        private static bool MissingNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod == null;

        private static bool InvalidNameMethod(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.ParameterList.Parameters.Count != 1 ||
            !twoFerSolution.InputParameter.Type.IsEquivalentWhenNormalized(
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
            twoFerSolution.NameMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Join")));

        private static bool UsesStringConcat(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(StringMemberAccessExpression(IdentifierName("Concat")));

        private static bool UsesStringReplace(this TwoFerSolution twoFerSolution) =>
            twoFerSolution.NameMethod.InvokesMethod(IdentifierName("Replace"));
    }
}