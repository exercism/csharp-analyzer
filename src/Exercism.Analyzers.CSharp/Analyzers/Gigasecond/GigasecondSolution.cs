using System;
using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        private readonly MethodDeclarationSyntax _addMethod;
        private readonly InvocationExpressionSyntax _addSecondsInvocationExpression;
        private readonly LocalDeclarationStatementSyntax _addSecondsLocalArgument;
        private readonly FieldDeclarationSyntax _addSecondsFieldArgument;
        private readonly ArgumentType _addSecondsArgumentType;
        private readonly GigasecondValueType _gigasecondValueType;
        private readonly ReturnType _addMethodReturnType;
        
        public GigasecondSolution(ParsedSolution solution,
            MethodDeclarationSyntax addMethod,
            ReturnType addMethodReturnType,
            InvocationExpressionSyntax addSecondsInvocationExpression,
            LocalDeclarationStatementSyntax addSecondsLocalArgument,
            FieldDeclarationSyntax addSecondsFieldArgument,
            ArgumentType addSecondsArgumentType,
            GigasecondValueType gigasecondValueType) : base(solution.Solution, solution.SyntaxRoot)
        {
            _addMethod = addMethod;
            _addSecondsInvocationExpression = addSecondsInvocationExpression;
            _addSecondsLocalArgument = addSecondsLocalArgument;
            _addSecondsFieldArgument = addSecondsFieldArgument;
            _addSecondsArgumentType = addSecondsArgumentType;
            _gigasecondValueType = gigasecondValueType;
            _addMethodReturnType = addMethodReturnType;
        }

        public string AddMethodName =>
            _addMethod.Identifier.Text;

        public bool UsesScientificNotation() =>
            _gigasecondValueType == GigasecondValueType.ScientificNotation;

        public bool UsesDigitsWithoutSeparator() =>
            _gigasecondValueType == GigasecondValueType.DigitsWithoutSeparator;

        public bool UsesDigitsWithSeparator() =>
            _gigasecondValueType == GigasecondValueType.DigitsWithSeparator;

        public bool UsesMathPow() =>
            _gigasecondValueType == GigasecondValueType.MathPow;

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