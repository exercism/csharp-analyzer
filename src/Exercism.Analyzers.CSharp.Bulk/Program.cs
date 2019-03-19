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
            var bulkSolutionsAnalysisRun = BulkSolutionsAnalyzer.Run(options);
            BulkSolutionAnalysisReport.Output(bulkSolutionsAnalysisRun);
            BulkSolutionsAnalysisRunWriter.Write(bulkSolutionsAnalysisRun);
        }
    }
}