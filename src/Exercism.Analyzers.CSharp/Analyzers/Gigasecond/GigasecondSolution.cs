using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        public MethodDeclarationSyntax AddMethod { get; }
        public IdentifierNameSyntax AddSecondsArgumentName { get; }
        public VariableDeclaratorSyntax AddSecondsArgumentVariable { get; }
        public ParameterSyntax BirthDateParameter { get; }
        private ClassDeclarationSyntax GigasecondClass { get; }
        private ExpressionSyntax ReturnedSingleLineExpression { get; }
        private ExpressionSyntax ReturnedVariableExpression { get; }
        private ExpressionSyntax ReturnedParameterExpression { get; }

        private ExpressionSyntax ReturnedExpression =>
            ReturnedSingleLineExpression ??
            ReturnedVariableExpression ??
            ReturnedParameterExpression;

        public bool UsesVariableInReturnedExpression => ReturnedVariableExpression != null;
        public bool UsesParameterInReturnedExpression => ReturnedParameterExpression != null;
        public bool UsesVariableInAddSecondsInvocation => AddSecondsArgumentName != null;

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            GigasecondClass = solution.SyntaxRoot.GetClass("Gigasecond");
            AddMethod = GigasecondClass.GetMethod("Add");
            BirthDateParameter = AddMethod?.ParameterList.Parameters.FirstOrDefault();
            ReturnedSingleLineExpression = AddMethod?.ExpressionDirectlyReturned();
            ReturnedVariableExpression = 
                AddMethod?.ExpressionAssignedToVariableAndReturned() ??
                AddMethod?.ExpressionUsesVariableAndReturned();
            ReturnedParameterExpression = AddMethod?.ExpressionAssignedToParameterAndReturned(BirthDateParameter);
            AddSecondsArgumentName = ReturnedExpression.AddSecondsArgumentName(BirthDateParameter);
            AddSecondsArgumentVariable = GigasecondClass.AssignedVariableWithName(AddSecondsArgumentName);
        }

        public bool Returns(SyntaxNode returned) =>
            ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}