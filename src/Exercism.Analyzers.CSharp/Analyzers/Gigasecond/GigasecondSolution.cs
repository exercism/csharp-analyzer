using System.Linq;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        
        public IdentifierNameSyntax AddSecondsArgumentName { get; }
        public VariableDeclaratorSyntax AddSecondsArgumentVariable { get; }
        public LocalDeclarationStatementSyntax AddSecondsArgumentVariableLocalDeclarationStatement;
        public FieldDeclarationSyntax AddSecondsArgumentVariableFieldDeclaration;
        
        
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

        public bool VariableIsPrivateField =>
            AddSecondsArgumentVariableFieldDeclaration != null &&
            AddSecondsArgumentVariableFieldDeclaration.IsPrivate();

        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            GigasecondClass = solution.SyntaxRoot.GetClass("Gigasecond");
            AddMethod = GigasecondClass.GetMethod("Add");

            if (AddMethod == null)
                return;
            
            AddMethodParameter = AddMethod.ParameterList.Parameters[0];
            AddSecondsArgumentExpression = AddMethod.DescendantNodes<InvocationExpressionSyntax>().FirstOrDefault(
                invocationExpression =>
                    invocationExpression.Expression.IsEquivalentWhenNormalized(
                        SimpleMemberAccessExpression(
                            IdentifierName(AddMethodParameter),
                            IdentifierName("AddSeconds"))))?.ArgumentList.Arguments[0].Expression;


            ReturnedSingleLineExpression = AddMethod?.ExpressionDirectlyReturned();
            ReturnedVariableExpression = AddMethod.ExpressionAssignedToVariableAndReturned();
            ReturnedParameterExpression = AddMethod?.ExpressionAssignedToParameterAndReturned(AddMethodParameter);
            AddSecondsArgumentName = ReturnedExpression.AddSecondsArgumentName(AddMethodParameter);
            AddSecondsArgumentVariable = GigasecondClass.AssignedVariableWithName(AddSecondsArgumentName);
            AddSecondsArgumentVariableLocalDeclarationStatement = AddSecondsArgumentVariable?.LocalDeclarationStatement();
            AddSecondsArgumentVariableFieldDeclaration = AddSecondsArgumentVariable?.FieldDeclaration();
            ReturnedUsingVariableExpression = AddMethod?.ExpressionUsesVariableAndReturned(AddSecondsArgumentVariable);
        }

        private ClassDeclarationSyntax GigasecondClass { get; }
        public MethodDeclarationSyntax AddMethod { get; }
        public ParameterSyntax AddMethodParameter { get; }    
        public ExpressionSyntax AddSecondsArgumentExpression { get; }

        public bool Returns(SyntaxNode returned) =>
            ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}