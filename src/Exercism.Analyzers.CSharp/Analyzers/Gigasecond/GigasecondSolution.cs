using System;

using Exercism.Analyzers.CSharp.Syntax;
using Exercism.Analyzers.CSharp.Syntax.Comparison;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Exercism.Analyzers.CSharp.Analyzers.Gigasecond.GigasecondSyntaxFactory;
using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedSyntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : Solution
    {
        public const string GigasecondClassName = "Gigasecond";
        public const string AddMethodName = "Add";
        public const string AddMethodSignature = "public static DateTime Add(DateTime moment)";

        public GigasecondSolution(Solution solution) : base(solution)
        {
        }

        private ClassDeclarationSyntax GigasecondClass =>
            SyntaxRoot.GetClass(GigasecondClassName);

        private MethodDeclarationSyntax AddMethod =>
            GigasecondClass?.GetMethod(AddMethodName);

        public bool MissingGigasecondClass =>
            GigasecondClass == null;

        public bool MissingAddMethod =>
            !MissingGigasecondClass && AddMethod == null;

        public bool InvalidAddMethod =>
            AddMethod != null &&
            (AddMethod.ParameterList.Parameters.Count != 1 ||
            !AddMethod.ParameterList.Parameters[0].Type.IsEquivalentWhenNormalized(
                IdentifierName("DateTime")) ||
            !AddMethod.ReturnType.IsEquivalentWhenNormalized(
                IdentifierName("DateTime")));

        private ReturnType AddMethodReturnType =>
            ReturnedAs(AddSecondsInvocationExpression, AddMethodReturnedExpression, AddMethodParameter);

        private ExpressionSyntax GigasecondValueExpression =>
            ArgumentValueExpression(AddSecondsArgumentType, AddSecondsArgumentExpression, AddSecondsArgumentVariable);

        private ArgumentType AddSecondsArgumentType =>
            ArgumentDefinedAs(AddSecondsFieldArgument, AddSecondsLocalArgument, AddSecondsArgumentExpression);

        private ExpressionSyntax AddSecondsArgumentExpression =>
            AddSecondsInvocationExpression?.FirstArgumentExpression();

        private VariableDeclaratorSyntax AddSecondsArgumentVariable =>
            GigasecondClass?.ArgumentVariable(AddSecondsArgumentExpression);

        private LocalDeclarationStatementSyntax AddSecondsLocalArgument =>
            AddSecondsArgumentVariable?.LocalDeclarationStatement();

        private FieldDeclarationSyntax AddSecondsFieldArgument =>
            AddSecondsArgumentVariable?.FieldDeclaration();

        private ExpressionSyntax AddMethodReturnedExpression =>
            AddMethod?.ReturnedExpression();

        private ParameterSyntax AddMethodParameter =>
            AddMethod?.FirstParameter();

        private InvocationExpressionSyntax AddSecondsInvocationExpression =>
            AddMethod?.InvocationExpression(AddSecondsMemberAccessExpression(this));

        public string AddMethodParameterName =>
            AddMethodParameter.Identifier.Text;

        public string GigasecondValue =>
            GigasecondValueExpression.ToFullString();

        public string GigasecondValueVariableName =>
            AddSecondsLocalArgument.Declaration.Variables[0].Identifier.Text;

        public string GigasecondValueFieldName =>
            AddSecondsFieldArgument.Declaration.Variables[0].Identifier.Text;

        public bool UsesScientificNotation =>
            GigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsScientificNotation());

        public bool UsesDigitsWithoutSeparator =>
            GigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithoutSeparator());

        public bool UsesDigitsWithSeparator =>
            GigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsDigitsWithSeparator());

        public bool UsesMathPow =>
            GigasecondValueExpression.IsEquivalentWhenNormalized(GigasecondAsMathPowInvocationExpression());

        public bool UsesExpressionBody =>
            AddMethod.IsExpressionBody();

        public bool UsesSingleLine =>
            AddMethod.SingleLine();

        public bool UsesConstField =>
            UsesField && AddSecondsFieldArgument.IsConst();

        public bool UsesPrivateField =>
            UsesField && AddSecondsFieldArgument.IsPrivate();

        public bool UsesField =>
            AddSecondsArgumentType == ArgumentType.Field;

        public bool DoesNotUseAddSeconds =>
            AddMethod != null &&
            AddSecondsInvocationExpression == null;

        public bool CreatesNewDatetime =>
            AddMethod != null &&
            AddMethod.CreatesObjectOfType<DateTime>();

        public bool UsesLocalConstVariable =>
            UsesLocalVariable &&
            AddSecondsLocalArgument.IsConst;

        public bool UsesLocalVariable =>
            AddSecondsArgumentType == ArgumentType.Local;

        public bool AssignsToParameterAndReturns =>
            AddMethodReturnType == ReturnType.ParameterAssigment;

        public bool AssignsToVariableAndReturns =>
            AddMethodReturnType == ReturnType.VariableAssignment;
    }
}