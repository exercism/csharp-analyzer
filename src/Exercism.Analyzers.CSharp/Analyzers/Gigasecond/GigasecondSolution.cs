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
        public LocalDeclarationStatementSyntax AddSecondsArgumentVariableLocalDeclarationStatement;
        public FieldDeclarationSyntax AddSecondsArgumentVariableFieldDeclaration;
        public ParameterSyntax BirthDateParameter { get; }
        private ClassDeclarationSyntax GigasecondClass { get; }
        private ExpressionSyntax ReturnedSingleLineExpression { get; }
        private ExpressionSyntax ReturnedVariableExpression { get; }
        private ExpressionSyntax ReturnedUsingVariableExpression { get; }
        private ExpressionSyntax ReturnedParameterExpression { get; }

        private ExpressionSyntax ReturnedExpression =>
            ReturnedSingleLineExpression ??
            ReturnedVariableExpression ??
            ReturnedUsingVariableExpression ??
            ReturnedParameterExpression;

        public bool ReturnsVariableInReturnedExpression => ReturnedVariableExpression != null;
        public bool UsesVariableInReturnedExpression => ReturnedUsingVariableExpression != null;
        public bool UsesParameterInReturnedExpression => ReturnedParameterExpression != null;
        public bool UsesVariableInAddSecondsInvocation => AddSecondsArgumentName != null;

        public bool VariableDefinedInLocalDeclaration => AddSecondsArgumentVariableLocalDeclarationStatement != null;
        public bool VariableDefinedInFieldDeclaration => AddSecondsArgumentVariableFieldDeclaration != null;

        public bool VariableIsConstant =>
            (AddSecondsArgumentVariableLocalDeclarationStatement?.IsConst ?? false) || 
            (AddSecondsArgumentVariableFieldDeclaration?.IsConst() ?? false);

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            GigasecondClass = solution.SyntaxRoot.GetClass("Gigasecond");
            AddMethod = GigasecondClass.GetMethod("Add");
            BirthDateParameter = AddMethod?.ParameterList.Parameters.FirstOrDefault();
            ReturnedSingleLineExpression = AddMethod?.ExpressionDirectlyReturned();
            ReturnedVariableExpression = AddMethod.ExpressionAssignedToVariableAndReturned();
            ReturnedUsingVariableExpression = AddMethod?.ExpressionUsesVariableAndReturned();
            ReturnedParameterExpression = AddMethod?.ExpressionAssignedToParameterAndReturned(BirthDateParameter);
            AddSecondsArgumentName = ReturnedExpression.AddSecondsArgumentName(BirthDateParameter);
            AddSecondsArgumentVariable = GigasecondClass.AssignedVariableWithName(AddSecondsArgumentName);
            AddSecondsArgumentVariableLocalDeclarationStatement = AddSecondsArgumentVariable?.LocalDeclarationStatement();
            AddSecondsArgumentVariableFieldDeclaration = AddSecondsArgumentVariable?.FieldDeclaration();
        }

        public bool Returns(SyntaxNode returned) =>
            ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}