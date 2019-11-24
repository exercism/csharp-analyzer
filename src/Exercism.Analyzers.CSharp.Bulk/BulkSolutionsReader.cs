using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercism.Analyzers.CSharp.Bulk
{
    internal static class BulkSolutionsReader
    {
        public static IEnumerable<BulkSolution> ReadAll(Options options) =>
            from solutionDirectory in GetSolutionDirectories(options.Directory)
            select CreateBulkSolution(options.Slug, solutionDirectory);

        private static IEnumerable<string> GetSolutionDirectories(string exerciseDirectory) =>
            Directory.GetDirectories(exerciseDirectory)
                .Where(IsHiddenDirectory)
                .OrderBy(directory => directory, StringComparer.Ordinal);

        private static bool IsHiddenDirectory(string directory) =>
            !Path.GetFileName(directory).StartsWith(".");

        private static BulkSolution CreateBulkSolution(string slug, string solutionDirectory) =>
            new BulkSolution(slug, solutionDirectory);
    }
}