using System.IO;
using Newtonsoft.Json;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    internal class FakeSolutionDirectory
    {
        private readonly FakeSolution _fakeSolution;
        private readonly DirectoryInfo _fakeSolutionDirectory;
        private readonly DirectoryInfo _fakeSolutionMetadataDirectory;
        private readonly string _implementationFileName;

        public FakeSolutionDirectory(FakeSolution fakeSolution)
        {
            _fakeSolution = fakeSolution;
            _fakeSolutionDirectory = GetFakeSolutionDirectory();
            _fakeSolutionMetadataDirectory = GetFakeSolutionMetadataDirectory();
            _implementationFileName = GetImplementationFileName();
        }

        public DirectoryInfo Directory => _fakeSolutionDirectory;

        private DirectoryInfo GetFakeSolutionDirectory() =>
            new DirectoryInfo(Path.Combine("Solutions", _fakeSolution.Exercise.Name, _fakeSolution.Category, _fakeSolution.ImplementationFile));
        
        private DirectoryInfo GetFakeSolutionMetadataDirectory() =>
            new DirectoryInfo(Path.Combine(GetFakeSolutionDirectory().FullName, ".exercism"));

        private string GetImplementationFileName() => $"{_fakeSolution.ImplementationFile}.cs";

        public void Create()
        {
            CreateSolutionDirectory();
            CreateSolutionFiles();
        }

        private void CreateSolutionDirectory()
        {
            _fakeSolutionDirectory.Recreate();
            _fakeSolutionMetadataDirectory.Recreate();
        }

        private void CreateSolutionFiles()
        {
            CreateMetadataFile();
            CopySolutionFile(_implementationFileName, ImplementationFileName);
            CopySolutionFile(TestFileName, TestFileName);
            CopySolutionFile(ProjectFileName, ProjectFileName);
        }

        private void CreateMetadataFile()
        {
            var metadata  = new
            {
                track = "csharp",
                exercise = _fakeSolution.Exercise.Slug,
                id = _fakeSolution.Id
            };
            var metadataFilePath = GetFakeSolutionMetadataFilePath(MetadataFileName);
            File.WriteAllText(metadataFilePath, JsonConvert.SerializeObject(metadata));
        }

        private void CopySolutionFile(string sourceSolutionFileName, string fakeSolutionFileName) =>
            File.Copy(GetSourceSolutionFilePath(sourceSolutionFileName), GetFakeSolutionFilePath(fakeSolutionFileName));

        private string GetSourceSolutionFilePath(string fileName) =>
            Path.Combine(_fakeSolution.Exercise.Name, _fakeSolution.Category, "Solutions", fileName);
        
        private string GetFakeSolutionFilePath(string fileName) =>
            Path.Combine(_fakeSolutionDirectory.FullName, fileName);
        
        private string GetFakeSolutionMetadataFilePath(string fileName) =>
            Path.Combine(_fakeSolutionMetadataDirectory.FullName, fileName);

        private string ImplementationFileName => $"{_fakeSolution.Exercise.Name}.cs";

        private string TestFileName => $"{_fakeSolution.Exercise.Name}Test.cs";

        private string ProjectFileName => $"{_fakeSolution.Exercise.Name}.csproj";

        private static string MetadataFileName => "metadata.json";
    }
}