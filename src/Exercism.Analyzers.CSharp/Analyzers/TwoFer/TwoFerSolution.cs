using Exercism.Analyzers.CSharp.Analyzers.Syntax.Comparison;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    internal class TwoFerSolution : ParsedSolution
    {
        private readonly TwoFerError _twoFerError;
        public ClassDeclarationSyntax TwoFerClass { get; }
        public MethodDeclarationSyntax SpeakMethod { get; }
        public ParameterSyntax InputMethodParameter { get; }
        public ExpressionSyntax TwoFerExpression { get; }
        public VariableDeclaratorSyntax TwoFerVariable { get; }
    
        public TwoFerSolution(ParsedSolution solution,
            ClassDeclarationSyntax twoFerClass,
            MethodDeclarationSyntax speakMethod,
            ParameterSyntax speakMethodParameter,
            ExpressionSyntax twoFerExpression,
            VariableDeclaratorSyntax twoFerVariableDeclarator,
            TwoFerError twoFerError) : base(solution.Solution, solution.SyntaxRoot)
        {
            _twoFerError = twoFerError;
            TwoFerClass = twoFerClass;
            SpeakMethod = speakMethod;
            InputMethodParameter = speakMethodParameter;
            TwoFerExpression = twoFerExpression;
            TwoFerVariable = twoFerVariableDeclarator;
        }

        public bool Returns(SyntaxNode returned) => TwoFerExpression.IsEquivalentWhenNormalized(returned);

        public bool MissingSpeakMethod =>
            _twoFerError == TwoFerError.MissingSpeakMethod;

        public bool InvalidSpeakMethod =>
            _twoFerError == TwoFerError.InvalidSpeakMethod;

        public bool UsesOverloads =>
            _twoFerError == TwoFerError.UsesOverloads;

        public bool UsesDuplicateString =>
            _twoFerError == TwoFerError.UsesDuplicateString;

        public bool UsesStringJoin =>
            _twoFerError == TwoFerError.UsesStringJoin;

        public bool UsesStringConcat =>
            _twoFerError == TwoFerError.UsesStringConcat;

        public bool UsesStringReplace =>
            _twoFerError == TwoFerError.UsesStringReplace;

        public bool NoDefaultValue =>
            _twoFerError == TwoFerError.NoDefaultValue;

        public bool InvalidDefaultValue =>
            _twoFerError == TwoFerError.InvalidDefaultValue;
    }
}