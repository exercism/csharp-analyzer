using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        public MethodDeclarationSyntax AddMethod { get; }
        public ParameterSyntax BirthDateParameter { get; }
        public ExpressionSyntax ReturnedSingleLineExpression { get; }
        public ExpressionSyntax ReturnedVariableExpression { get; }
        public ExpressionSyntax ReturnedParameterExpression { get; }

        public ArgumentSyntax AddSecondsArgument { get; }

        public ExpressionSyntax ReturnedExpression =>
            ReturnedSingleLineExpression ??
            ReturnedVariableExpression ??
            ReturnedParameterExpression;

        public bool UsesVariableInReturnedExpression => ReturnedVariableExpression != null;
        public bool UsesParameterInReturnedExpression => ReturnedParameterExpression != null;

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            AddMethod = solution.SyntaxRoot.GetClassMethod("Gigasecond", "Add");
            BirthDateParameter = AddMethod?.ParameterList.Parameters.FirstOrDefault();
            ReturnedSingleLineExpression = AddMethod?.ExpressionDirectlyReturned();
            ReturnedVariableExpression = AddMethod?.ExpressionAssignedToVariableAndReturned();
            ReturnedParameterExpression = AddMethod?.ExpressionAssignedToParameterAndReturned(BirthDateParameter);
            AddSecondsArgument = ReturnedExpression.AddSecondsArgument(BirthDateParameter);
        }

        public bool Returns(SyntaxNode returned) =>
            ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}