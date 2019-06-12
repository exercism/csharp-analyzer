using Exercism.Analyzers.CSharp.Analyzers.Syntax;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exercism.Analyzers.CSharp.Analyzers.Leap
{
    internal static class LeapSolutionParser
    {
        public static LeapSolution Parse(ParsedSolution solution)
        {
            var leapClass = solution.LeapClass();
            var isLeapYearMethod = leapClass.IsLeapYearMethod();
            var yearParameter = isLeapYearMethod.YearParameter();
            var returnedExpression = isLeapYearMethod.ReturnedExpression();

            return new LeapSolution(solution, isLeapYearMethod, yearParameter, returnedExpression);
        }

        private static ClassDeclarationSyntax LeapClass(this ParsedSolution solution) =>
            solution.SyntaxRoot.GetClass("Leap");

        private static MethodDeclarationSyntax IsLeapYearMethod(this ClassDeclarationSyntax leapClass) =>
            leapClass?.GetMethod("IsLeapYear");

        private static ParameterSyntax YearParameter(this MethodDeclarationSyntax isLeapYearMethod) =>
            isLeapYearMethod.ParameterList?.Parameters.FirstOrDefault();
    }
}
