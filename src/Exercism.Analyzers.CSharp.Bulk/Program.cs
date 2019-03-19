using System;
using CommandLine;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Logging.Configure();

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Analyze);
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