using System;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Bulk
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Logging.Configure();

            await Analyze(new Options(args));
        }

        private static async Task Analyze(Options options)
        {
            var bulkSolutions = BulkSolutionsReader.ReadAll(options.Slug, options.Directory);
            var bulkSolutionsAnalysisRun = await BulkSolutionsAnalyzer.Run(bulkSolutions);
            var bulkSolutionsAnalysisReport = BulkSolutionAnalysisReport.Create(bulkSolutionsAnalysisRun, options);
            
            Console.WriteLine(bulkSolutionsAnalysisReport);
        }
    }
}