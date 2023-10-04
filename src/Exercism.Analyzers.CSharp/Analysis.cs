using System.Collections.Generic;

using Exercism.Analyzers.CSharp.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exercism.Analyzers.CSharp;

internal record Analysis(List<Comment> Comments, List<string> Tags)
{
    public static Analysis Empty => new(new List<Comment>(), new List<string>());
}

internal abstract class Analyzer : CSharpSyntaxWalker
{
    protected Submission Submission;
    protected Compilation Compilation => Submission.Compilation;
    protected SemanticModel SemanticModel;
    protected Analysis Analysis;

    protected Analyzer(Submission submission) => Submission = submission;

    public static Analysis Analyze(Submission submission)
    {
        if (submission.HasCompilationErrors)
            return Analysis.Empty; // We can't really analyze the solution when there are compilation errors

        var analysis = Analysis.Empty;

        foreach (var analyzer in AnalyzerFactory.CreateAnalyzers(submission))
            analyzer.Analyze(analysis);

        return analysis;
    }

    protected void AddComment(Comment comment) => Analysis.Comments.Add(comment);

    protected void AddTags(params string[] tags)
    {
        foreach (var tag in tags)
            Analysis.Tags.Add(tag);
    }

    private void Analyze(Analysis analysis)
    {
        Analysis = analysis;

        foreach (var syntaxTree in Compilation.SyntaxTrees)
        {
            SemanticModel = Compilation.GetSemanticModel(syntaxTree);
            Visit(syntaxTree.GetRoot());
        }
    }

    private static class AnalyzerFactory
    {
        public static IEnumerable<Analyzer> CreateAnalyzers(Submission submission)
        {
            switch (submission.Slug)
            {
                case "collatz-conjecture":
                    yield return new CollatzConjectureAnalyzer(submission);
                    break;
                case "difference-of-squares":
                    yield return new DifferenceOfSquaresAnalyzer(submission);
                    break;
                case "gigasecond":
                    yield return new GigasecondAnalyzer(submission);
                    break;
                case "grains":
                    yield return new GrainsAnalyzer(submission);
                    break;
                case "isogram":
                    yield return new IsogramAnalyzer(submission);
                    break;
                case "pangram":
                    yield return new PangramAnalyzer(submission);
                    break;
                case "parallel-letter-frequency":
                    yield return new ParallelLetterFrequencyAnalyzer(submission);
                    break;
                case "protein-translation":
                    yield return new ProteinTranslationAnalyzer(submission);
                    break;
                case "leap":
                    yield return new LeapAnalyzer(submission);
                    break;
                case "raindrops":
                    yield return new RaindropsAnalyzer(submission);
                    break;
                case "reverse-string":
                    yield return new ReverseStringAnalyzer(submission);
                    break;
                case "two-fer":
                    yield return new TwoFerAnalyzer(submission);
                    break;
                case "weighing-machine":
                    yield return new WeighingMachineAnalyzer(submission);
                    break;
            }

            yield return new CommonAnalyzer(submission);
            yield return new TagAnalyzer(submission);
        }
    }

    protected SymbolInfo GetSymbolInfo(SyntaxNode node) => SemanticModel.GetSymbolInfo(node);
    protected ISymbol GetSymbol(SyntaxNode node) => GetSymbolInfo(node).Symbol;
    protected string GetSymbolName(SyntaxNode node) => GetSymbol(node)?.ToDisplayString();
    
    protected ISymbol GetDeclaredSymbol(SyntaxNode node) => SemanticModel.GetDeclaredSymbol(node);
    protected string GetDeclaredSymbolName(SyntaxNode node) => GetDeclaredSymbol(node)?.ToDisplayString();

    protected TypeInfo GetTypeInfo(SyntaxNode node) => SemanticModel.GetTypeInfo(node);

    protected IMethodSymbol GetConstructedFromSymbol(SyntaxNode node) =>
        GetSymbol(node) is IMethodSymbol methodSymbol ? methodSymbol.ConstructedFrom : null;
    protected string GetConstructedFromSymbolName(SyntaxNode node) => GetConstructedFromSymbol(node)?.ToDisplayString();
}