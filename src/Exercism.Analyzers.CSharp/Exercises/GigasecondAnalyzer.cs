using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Exercises;

internal class GigasecondAnalyzer : ExerciseAnalyzer
{
    protected override void AnalyzeExerciseSpecific(Solution solution)
    {
        var visitor = new SyntaxWalker(solution.Compilation);

        foreach (var syntaxTree in solution.Compilation.SyntaxTrees)
            visitor.Visit(syntaxTree.GetRoot());

        if (!visitor.UsesDateTimeAddSeconds)
            Analysis.Comments.Add(Comments.UseAddSeconds);
        
        base.Analyze(solution);
    }

    private class SyntaxWalker : CSharpSyntaxWalker
    {
        private readonly Compilation _compilation;

        public bool UsesDateTimeAddSeconds;

        public SyntaxWalker(Compilation compilation) => _compilation = compilation;

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree);
            var invocationSymbol = semanticModel.GetSymbolInfo(node);

            if (invocationSymbol.ToString() == "System.DateTime.AddSeconds(double)")
                UsesDateTimeAddSeconds = true;

            base.VisitInvocationExpression(node);
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