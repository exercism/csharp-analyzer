using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.CommandLine;
using Exercism.Analyzers.CSharp.Analysis.Solutions;

namespace Exercism.Analyzers.CSharp.Tests.Analysis.Solutions
{
    internal class FakeExercismCommandLineInterface : ExercismCommandLineInterface
    {
        private readonly ConcurrentDictionary<string, DirectoryInfo> _fakeSolutionDirectories = new ConcurrentDictionary<string, DirectoryInfo>();

        public override Task<DirectoryInfo> Download(string id)
        {
            if (_fakeSolutionDirectories.TryGetValue(id, out var directoryInfo))
                return Task.FromResult(directoryInfo);

            return Task.FromResult(new DirectoryInfo(""));
        }
        
        public void Configure(Solution solution, string implementationFileSuffix)
        {
            var fakeSolution = new FakeSolution(solution, implementationFileSuffix);
            var fakeSolutionDirectory = fakeSolution.Create();
            _fakeSolutionDirectories.AddOrUpdate(solution.Id, fakeSolutionDirectory, (_, value) => value);
        }
    }
}