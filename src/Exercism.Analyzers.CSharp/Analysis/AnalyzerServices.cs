using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.Extensions.DependencyInjection;

namespace Exercism.Analyzers.CSharp.Analysis
{
    internal static class AnalyzerServices
    {
        public static IServiceCollection AddAnalyzer(this IServiceCollection services) =>
            services.AddSingleton<Analyzer>()
                    .AddSingleton<SolutionDownloader>()
                    .AddSingleton<SolutionLoader>()
                    .AddSingleton<SolutionCompiler>();
    }
}