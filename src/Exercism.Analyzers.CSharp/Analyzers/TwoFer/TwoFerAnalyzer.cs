using Exercism.Analyzers.CSharp.Analyzers.Shared;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.TwoFer.TwoFerSyntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal static class TwoFerAnalyzer
    {   
        public static SolutionAnalysis Analyze(ParsedSolution parsedSolution)
        {
            var twoFerSolution = new TwoFerSolution(parsedSolution);

            return 
                twoFerSolution.AnalyzeError() ??
                twoFerSolution.AnalyzeSingleLine() ??
                twoFerSolution.AnalyzeMultiLine() ??
                twoFerSolution.ReferToMentor();
        }

        private static SolutionAnalysis AnalyzeError(this TwoFerSolution twoFerSolution)
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

            if (twoFerSolution.AssignsToParameter())
                return twoFerSolution.DisapproveWithComment(SharedComments.DontAssignToParameter);

            return null;
        }

        private static SolutionAnalysis AnalyzeSingleLine(this TwoFerSolution twoFerSolution)
        {
            if (!twoFerSolution.NameMethod.SingleExpression())
                return null;

            if (twoFerSolution.NameMethod.IsExpressionBody() &&
                (twoFerSolution.ReturnedExpression.IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                 twoFerSolution.ReturnedExpression.IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter)))
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.ReturnedExpression.IsDefaultInterpolatedStringExpression(twoFerSolution.InputParameter) ||
                twoFerSolution.ReturnedExpression.IsNullCoalescingInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(SharedComments.UseExpressionBodiedMember);

            if (twoFerSolution.ReturnedExpression.IsIsNullOrEmptyInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.ReturnedExpression.IsIsNullOrWhiteSpaceInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            if (twoFerSolution.ReturnedExpression.IsTernaryOperatorInterpolatedStringExpression(twoFerSolution.InputParameter))
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.UsesStringConcatenation())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (twoFerSolution.UsesStringFormat())
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            return null;
        }

        private static SolutionAnalysis AnalyzeMultiLine(this TwoFerSolution twoFerSolution)
        {
            var variableDeclarator = twoFerSolution.NameMethod.AssignedVariable();
            if (variableDeclarator == null)
                return null;

            if (!twoFerSolution.AssignsVariableUsingKnownInitializer())
                return twoFerSolution.ReferToMentor();

            if (twoFerSolution.ReturnedExpression.IsSafeEquivalentTo(
                StringFormatInvocationExpression(
                    IdentifierName(variableDeclarator.Identifier))))
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringFormat);

            if (twoFerSolution.ReturnedExpression.IsSafeEquivalentTo(
                StringConcatenationExpression(
                    IdentifierName(variableDeclarator.Identifier))))
                return twoFerSolution.ApproveWithComment(SharedComments.UseStringInterpolationNotStringConcatenation);

            if (!twoFerSolution.ReturnedExpression.IsSafeEquivalentTo(CreateInterpolatedStringExpression(Interpolation(
                IdentifierName(variableDeclarator.Identifier)))))
                return null;

            if (twoFerSolution.VariableAssignedUsingNullCoalescingOperator())
                return twoFerSolution.ApproveAsOptimal();

            if (twoFerSolution.VariableAssignedUsingTernaryOperator())
                return twoFerSolution.ApproveWithComment(SharedComments.UseNullCoalescingOperatorNotTernaryOperatorWithNullCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrEmpty())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrEmptyCheck);

            if (twoFerSolution.VariableAssignedUsingIsNullOrWhiteSpace())
                return twoFerSolution.ApproveWithComment(TwoFerComments.UseNullCoalescingOperatorNotTernaryOperatorWithIsNullOrWhiteSpaceCheck);

            return null;
        }

        
    }
}