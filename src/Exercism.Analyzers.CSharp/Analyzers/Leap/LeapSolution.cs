using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal class LeapSolution : ParsedSolution
    {
        public MethodDeclarationSyntax IsLeapYearMethod { get; }
        public ParameterSyntax YearParameter { get; }
        public ExpressionSyntax ReturnedExpression { get; }

        public LeapSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            IsLeapYearMethod = solution.SyntaxRoot.GetClassMethod("Leap", "IsLeapYear");
            YearParameter = IsLeapYearMethod.ParameterList?.Parameters.FirstOrDefault();
            ReturnedExpression = IsLeapYearMethod?.ReturnedExpression();
        }

        public bool Returns(SyntaxNode returned) => ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}