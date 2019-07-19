using System;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : Solution
    {
        private readonly ClassDeclarationSyntax _gigasecondClass;
        private readonly MethodDeclarationSyntax _addMethod;
        private readonly ParameterSyntax _addMethodParameter;
        private readonly ReturnType _addMethodReturnType;
        private readonly ExpressionSyntax _addMethodReturnedExpression;
        private readonly InvocationExpressionSyntax _addSecondsInvocationExpression;
        private readonly ExpressionSyntax _addSecondsArgumentExpression;
        private readonly LocalDeclarationStatementSyntax _addSecondsLocalArgument;
        private readonly FieldDeclarationSyntax _addSecondsFieldArgument;
        private readonly VariableDeclaratorSyntax _addSecondsArgumentVariable;
        private readonly ArgumentType _addSecondsArgumentType;
        private readonly ExpressionSyntax _gigasecondValueExpression;

        public GigasecondSolution(Solution solution) : base(solution)
        {
            _gigasecondClass = GigasecondClass();
            _addMethod = AddMethod();
            _addMethodParameter = AddMethodParameter();
            _addMethodReturnedExpression = AddMethodReturnedExpression();
            _addSecondsInvocationExpression = AddSecondsInvocationExpression();
            _addSecondsArgumentExpression = AddSecondsArgumentExpression();
            _addSecondsArgumentVariable = AddSecondsArgumentVariable();
            _addSecondsLocalArgument = AddSecondsLocalArgument();
            _addSecondsFieldArgument = AddSecondsFieldArgument();
            _addSecondsArgumentType = AddSecondsArgumentType();
            _gigasecondValueExpression = GigasecondValueExpression();
            _addMethodReturnType = AddMethodReturnType();
        }

        private ClassDeclarationSyntax GigasecondClass() =>
            SyntaxRoot.GetClass("Gigasecond");

        private MethodDeclarationSyntax AddMethod() =>
            _gigasecondClass?.GetMethod("Add");

        private ReturnType AddMethodReturnType() =>
            ReturnedAs(_addSecondsInvocationExpression, _addMethodReturnedExpression, _addMethodParameter);

        private ExpressionSyntax GigasecondValueExpression() =>
            ArgumentValueExpression(_addSecondsArgumentType, _addSecondsArgumentExpression, _addSecondsArgumentVariable);

        private ArgumentType AddSecondsArgumentType() =>
            ArgumentDefinedAs(_addSecondsFieldArgument, _addSecondsLocalArgument, _addSecondsArgumentExpression);

        private ExpressionSyntax AddSecondsArgumentExpression() =>
            _addSecondsInvocationExpression.FirstArgumentExpression();

        private VariableDeclaratorSyntax AddSecondsArgumentVariable() =>
            _gigasecondClass.ArgumentVariable(_addSecondsArgumentExpression);

        private LocalDeclarationStatementSyntax AddSecondsLocalArgument() =>
            _addSecondsArgumentVariable.LocalDeclarationStatement();

        private FieldDeclarationSyntax AddSecondsFieldArgument() =>
            _addSecondsArgumentVariable.FieldDeclaration();

        private ExpressionSyntax AddMethodReturnedExpression() =>
            _addMethod.ReturnedExpression();

        private ParameterSyntax AddMethodParameter() =>
            _addMethod.FirstParameter();

        private InvocationExpressionSyntax AddSecondsInvocationExpression() =>
            _addMethod.InvocationExpression(AddSecondsMemberAccessExpression(this));

        public string AddMethodName =>
            _addMethod.Identifier.Text;

        public string AddMethodParameterName =>
            _addMethodParameter.Identifier.Text;

        public string GigasecondValue =>
            _gigasecondValueExpression.ToFullString();

        public string GigasecondValueVariableName =>
            _addSecondsLocalArgument.Declaration.Variables[0].Identifier.Text;

        public string GigasecondValueFieldName =>
            _addSecondsFieldArgument.Declaration.Variables[0].Identifier.Text;

        public bool UsesScientificNotation() =>
            _gigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsScientificNotation());

        public bool UsesDigitsWithoutSeparator() =>
            _gigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithoutSeparator());

        public bool UsesDigitsWithSeparator() =>
            _gigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithSeparator());

        public bool UsesMathPow() =>
            _gigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsMathPowInvocationExpression());

        public bool UsesExpressionBody() =>
            _addMethod.IsExpressionBody();

        public bool UsesSingleLine() =>
            _addMethod.SingleLine();

        public bool UsesConstField() =>
            UsesField() &&
            _addSecondsFieldArgument.IsConst();

        public bool UsesPrivateField() =>
            UsesField() &&
            _addSecondsFieldArgument.IsPrivate();

        public bool UsesField() =>
            _addSecondsArgumentType == ArgumentType.Field;

        public bool DoesNotUseAddSeconds() =>
            _addSecondsInvocationExpression == null;

        public bool CreatesNewDatetime() =>
            _addMethod.CreatesObjectOfType<DateTime>();

        public bool UsesLocalConstVariable() =>
            UsesLocalVariable() &&
            _addSecondsLocalArgument.IsConst;

        public bool UsesLocalVariable() =>
            _addSecondsArgumentType == ArgumentType.Local;

        public bool AssignsToParameterAndReturns() =>
            _addMethodReturnType == ReturnType.ParameterAssigment;

        public bool AssignsToVariableAndReturns() =>
            _addMethodReturnType == ReturnType.VariableAssignment;
    }
}