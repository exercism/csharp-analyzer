using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal class LeapSolution : ParsedSolution
    {
        public readonly ParameterSyntax YearParameter;
        public readonly MethodDeclarationSyntax IsLeapYearMethod;
        public readonly SyntaxNode ReturnExpression;

        public LeapSolution(ParsedSolution solution,
            MethodDeclarationSyntax isLeapYearMethod,
            ParameterSyntax yearParameter,
            SyntaxNode returnExpression) : base(solution.Solution, solution.SyntaxRoot)
        {
            YearParameter = yearParameter;
            IsLeapYearMethod = isLeapYearMethod;
            ReturnExpression = returnExpression;
        }

        public bool Returns(SyntaxNode returned) => ReturnExpression.IsEquivalentWhenNormalized(returned);
    }
}