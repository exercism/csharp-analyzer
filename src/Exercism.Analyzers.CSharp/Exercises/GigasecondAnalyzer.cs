using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Exercises;

internal class GigasecondAnalyzer : ExerciseAnalyzer
{
    protected override void AnalyzeExerciseSpecific(Solution solution)
    {
        var visitor = new SyntaxWalker(solution.Compilation, Analysis);

        foreach (var syntaxTree in solution.Compilation.SyntaxTrees)
            visitor.Visit(syntaxTree.GetRoot());
    }

    private class SyntaxWalker : CSharpSyntaxWalker
    {
        private readonly Compilation _compilation;
        private readonly Analysis _analysis;

        public SyntaxWalker(Compilation compilation, Analysis analysis) =>
            (_compilation, _analysis) = (compilation, analysis);

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree);
            var invocationSymbol = semanticModel.GetSymbolInfo(node);

            switch (invocationSymbol.Symbol?.ToString())
            {
                case "System.DateTime.AddSeconds(double)":
                    _analysis.Comments.Add(Comments.UseAddSeconds);

                    var argument = node.ArgumentList.Arguments[0].Expression.ToString();
                    if (argument.Contains('e', StringComparison.OrdinalIgnoreCase) || !argument.Contains("_"))
                        _analysis.Comments.Add((Comments.UseScientificNotationOrDigitSeparators(argument)));

                    break;
                case "System.Math.Pow(double, double)" when node.ArgumentList.ToString() == "(10,9)":
                    _analysis.Comments.Add(Comments.UseScientificNotationNotMathPow(node.ToString()));
                    break;
            }

            base.VisitInvocationExpression(node);
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree);
            var methodSymbol = semanticModel.GetSymbolInfo(node);

            if (methodSymbol.ToString() == "System.DateTime.DateTime(long)")
                _analysis.Comments.Add(Comments.DoNotCreateDateTime);
            
            base.VisitObjectCreationExpression(node);
        }
    }

    private static class Comments
    {
        public static readonly Comment UseAddSeconds = new("csharp.gigasecond.use_add_seconds", CommentType.Actionable);
        public static readonly Comment DoNotCreateDateTime = new("csharp.gigasecond.do_not_create_datetime", CommentType.Essential);

        public static Comment UseScientificNotationNotMathPow(string gigasecondValue) =>
            new("csharp.gigasecond.use_1e9_not_math_pow", CommentType.Informative, new CommentParameter("value", gigasecondValue));

        public static Comment UseScientificNotationOrDigitSeparators(string gigasecondValue) =>
            new("csharp.gigasecond.use_1e9_or_digit_separator", CommentType.Informative, new CommentParameter("value", gigasecondValue));
    }
}