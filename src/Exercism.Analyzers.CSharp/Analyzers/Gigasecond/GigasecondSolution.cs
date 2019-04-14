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
        public ExpressionSyntax ReturnedVariableExpression { get; }

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            AddMethod = solution.SyntaxRoot.GetClassMethod("Gigasecond", "Add");
            BirthDateParameter = AddMethod?.ParameterList.Parameters.FirstOrDefault();
            ReturnedExpression = AddMethod?.ReturnedExpression();
            ReturnedVariableExpression = AddMethod?.AssignedAndReturnedExpression();
        }

        public bool Returns(SyntaxNode returned) =>
            ReturnedExpression.IsEquivalentWhenNormalized(returned);

        public bool AssignsAndReturns(SyntaxNode returned) =>
            ReturnedVariableExpression.IsEquivalentWhenNormalized(returned);
    }
}