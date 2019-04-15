using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal class TwoFerSolution : ParsedSolution
    {
        public ClassDeclarationSyntax TwoFerClass { get; }
        public MethodDeclarationSyntax SpeakMethod { get; }
        public ParameterSyntax InputParameter { get; }
        public ExpressionSyntax ReturnedExpression { get; }
        public VariableDeclaratorSyntax Variable { get; }
    
        public TwoFerSolution(ParsedSolution solution) : base(solution.Solution, solution.SyntaxRoot)
        {
            TwoFerClass = solution.SyntaxRoot.GetClass("TwoFer");
            SpeakMethod = TwoFerClass.GetMethod("Speak");
            InputParameter = SpeakMethod?.ParameterList.Parameters.FirstOrDefault();
            ReturnedExpression = SpeakMethod?.ReturnedExpression();
            Variable = SpeakMethod?.AssignedVariable();
        }

        public bool Returns(SyntaxNode returned) => ReturnedExpression.IsEquivalentWhenNormalized(returned);
    }
}