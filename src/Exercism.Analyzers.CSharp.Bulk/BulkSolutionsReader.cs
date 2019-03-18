using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class BulkSolutionsReader
    {
        public static IEnumerable<BulkSolution> ReadAll(string slug, string exerciseDirectory) =>
            from solutionDirectory in GetSolutionDirectories(exerciseDirectory)
            select CreateBulkSolution(slug, solutionDirectory);

        private static string[] GetSolutionDirectories(string exerciseDirectory) =>
            Directory.GetDirectories(exerciseDirectory);

        private static BulkSolution CreateBulkSolution(string slug, string solutionDirectory) =>
            new BulkSolution(slug, solutionDirectory);
    }
}