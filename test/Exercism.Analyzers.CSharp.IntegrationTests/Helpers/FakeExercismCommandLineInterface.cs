using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.CommandLine;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.Extensions.Logging.Abstractions;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    internal class FakeExercismCommandLineInterface : ExercismCommandLineInterface
    {
        private readonly ConcurrentDictionary<string, DirectoryInfo> _fakeSolutionDirectories = new ConcurrentDictionary<string, DirectoryInfo>();

        public FakeExercismCommandLineInterface() : base(new NullLogger<ExercismCommandLineInterface>())
        {
        }

        public override Task<DirectoryInfo> Download(string id)
        {
            if (_fakeSolutionDirectories.TryGetValue(id, out var directoryInfo))
                return Task.FromResult(directoryInfo);

            return Task.FromResult(new DirectoryInfo(""));
        }
        
        public void Configure(FakeSolution fakeSolution)
        {
            var fakeSolutionDirectory = new FakeSolutionDirectory(fakeSolution);
            fakeSolutionDirectory.Create();

            _fakeSolutionDirectories.AddOrUpdate(fakeSolution.Id, fakeSolutionDirectory.Directory, (_, value) => value);
        }
    }
}