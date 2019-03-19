using System;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.Configure();

            // TODO: use library to parse options
            Analyze(new Options(args));
        }

        private static void Analyze(Options options)
        {
            var bulkSolutions = BulkSolutionsReader.ReadAll(options.Slug, options.Directory);
            var bulkSolutionsAnalysisRun = BulkSolutionsAnalyzer.Run(bulkSolutions);
            var bulkSolutionsAnalysisReport = BulkSolutionAnalysisReport.Create(bulkSolutionsAnalysisRun, options);
            
            Console.WriteLine(bulkSolutionsAnalysisReport);
        }
    }
}