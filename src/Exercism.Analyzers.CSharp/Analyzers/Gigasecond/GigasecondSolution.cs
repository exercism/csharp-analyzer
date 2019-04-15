using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        private readonly ExpressionSyntax _returnedSingleLineExpression;
        private readonly ExpressionSyntax _returnedVariableExpression;
        private readonly ExpressionSyntax _returnedParameterExpression;
        
        public MethodDeclarationSyntax AddMethod { get; }
        public ParameterSyntax BirthDateParameter { get; }

        public bool UsesVariableInReturnedExpression => _returnedVariableExpression != null;
        public bool UsesParameterInReturnedExpression => _returnedParameterExpression != null;

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            AddMethod = solution.SyntaxRoot.GetClassMethod("Gigasecond", "Add");
            BirthDateParameter = AddMethod?.ParameterList.Parameters.FirstOrDefault();
            _returnedSingleLineExpression = AddMethod?.ExpressionDirectlyReturned();
            _returnedVariableExpression = AddMethod?.ExpressionAssignedToVariableAndReturned();
            _returnedParameterExpression = AddMethod?.ExpressionAssignedToParameterAndReturned(BirthDateParameter);
        }

        public bool Returns(SyntaxNode returned) =>
            _returnedSingleLineExpression.IsEquivalentWhenNormalized(returned) ||
            _returnedVariableExpression.IsEquivalentWhenNormalized(returned) ||
            _returnedParameterExpression.IsEquivalentWhenNormalized(returned);
    }
}