using System;
using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    internal class FakeSolutionDownloader : SolutionDownloader
    {
        protected override Task<DirectoryInfo> DownloadToDirectory(Solution solution)
        {
            if (solution.Uuid == StubSolution.WithoutDiagnostics.Uuid)
                return DirectoryForSampleSolution("no-diagnostics-tests");
            if (solution.Uuid == StubSolution.WithDiagnostics.Uuid)
                return DirectoryForSampleSolution("with-diagnostics-tests");
            
            throw new InvalidOperationException($"Could not download exercise with UUID {solution.Uuid}");
        }

        private static Task<DirectoryInfo> DirectoryForSampleSolution(string failingTests) 
            => Task.FromResult(new DirectoryInfo(Path.Combine("samples", failingTests)));
    }
}