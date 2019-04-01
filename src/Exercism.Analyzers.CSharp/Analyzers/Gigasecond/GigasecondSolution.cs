using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        public MethodDeclarationSyntax AddMethod { get; }
        public ParameterSyntax BirthDateParameter { get; }
        public ExpressionSyntax ReturnedExpression { get; }

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            AddMethod = solution.SyntaxRoot.GetClassMethod("Gigasecond", "Add");
            BirthDateParameter = AddMethod?.ParameterList.Parameters.FirstOrDefault();
            ReturnedExpression = AddMethod?.ReturnedExpression();
        }

        public bool Returns(SyntaxNode returned) => ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}