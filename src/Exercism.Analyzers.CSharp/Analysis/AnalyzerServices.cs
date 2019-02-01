using Exercism.Analyzers.CSharp.Analysis.CommandLine;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace Exercism.Analyzers.CSharp.Analysis
{
    internal static class AnalyzerServices
    {
        public static void AddAnalyzer(this IServiceCollection services) =>
            services.AddSingleton<Analyzer>()
                    .AddSingleton<SolutionDownloader>()
                    .AddSingleton<SolutionCompiler>()
                    .AddSingleton<SolutionLoader>()
                    .AddSingleton<SolutionAnalyzer>()
                    .AddSingleton<ExercismCommandLineInterface>();
    }
}