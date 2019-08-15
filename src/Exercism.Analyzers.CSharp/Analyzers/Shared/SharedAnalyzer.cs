using static Exercism.Analyzers.CSharp.Analyzers.Shared.SharedComments;

namespace Exercism.Analyzers.CSharp.Analyzers.Shared
{
    internal static class SharedAnalyzer
    {
        public static SolutionAnalysis Analyze(Solution solution)
        {
            if (solution.NoImplementationFileFound())
                return solution.ReferToMentor();

            if (solution.HasCompileErrors())
                solution.AddComment(FixCompileErrors);

            if (solution.HasMainMethod())
                solution.AddComment(RemoveMainMethod);

            if (solution.ThrowsNotImplementedException())
                solution.AddComment(RemoveThrowNotImplementedException);

            if (solution.WritesToConsole())
                solution.AddComment(DoNotWriteToConsole);

            return solution.HasComments
                ? solution.Disapprove()
                : solution.ContinueAnalysis();
        }
    }
}