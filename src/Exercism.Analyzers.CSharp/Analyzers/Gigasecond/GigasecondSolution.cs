using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal class GigasecondSolution : ParsedSolution
    {
        public GigasecondSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            GigasecondClass = solution.Class();
            AddMethod = this.AddMethod();
            AddMethodParameter = this.AddMethodParameter();
            AddMethodReturnedExpression = AddMethod.ReturnedExpression();
            AddSecondsInvocationExpression = this.AddSecondsInvocationExpression();
            AddSecondsArgumentExpression = this.AddSecondsArgumentExpression();
            AddSecondsArgumentVariable = this.AddSecondsArgumentVariable();
            AddSecondsArgumentVariableFieldDeclaration = AddSecondsArgumentVariable.FieldDeclaration();
            AddSecondsArgumentVariableLocalDeclarationStatement = AddSecondsArgumentVariable.LocalDeclarationStatement();
            AddSecondsArgumentType = this.AddSecondsArgumentDefinedAs();
            AddSecondsArgumentValueExpression = this.AddSecondsArgumentValueExpression();
            AddSecondsReturnType = this.AddSecondsReturnedAs();
            AddSecondsArgumentValueType = this.AddSecondsArgumentValueType();
        }

        public ClassDeclarationSyntax GigasecondClass { get; }
        public MethodDeclarationSyntax AddMethod { get; }
        public ParameterSyntax AddMethodParameter { get; }
        public ExpressionSyntax AddMethodReturnedExpression { get; }
        public InvocationExpressionSyntax AddSecondsInvocationExpression { get; }
        public ExpressionSyntax AddSecondsArgumentExpression { get; }
        public ExpressionSyntax AddSecondsArgumentValueExpression { get; }
        public VariableDeclaratorSyntax AddSecondsArgumentVariable { get; }
        public LocalDeclarationStatementSyntax AddSecondsArgumentVariableLocalDeclarationStatement { get; }
        public FieldDeclarationSyntax AddSecondsArgumentVariableFieldDeclaration { get; }
        public ArgumentType AddSecondsArgumentType { get; }
        public AddSecondsArgumentValueType AddSecondsArgumentValueType { get; }
        public ReturnType AddSecondsReturnType { get; }
    }
}