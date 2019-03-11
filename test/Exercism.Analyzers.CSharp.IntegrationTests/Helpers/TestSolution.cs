using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolution : Solution
    {
        public TestSolution(string exercise) : this(exercise, "csharp")
        {
        }

        public TestSolution(string exercise, string track) : base(exercise, track, Path.Combine("solutions", track, exercise))
        {
        }

        public void CreateFiles(string code)
        {
            CreateDirectory();
            CreateImplementationFile(code);
            CreateSolutionFile();
        }

        private void CreateDirectory()
        {
            if (Directory.Exists(Paths.Directory))
                Directory.Delete(Paths.Directory, recursive: true);

            Directory.CreateDirectory(Paths.Directory);
        }

        private void CreateImplementationFile(string code) =>
            CreateFile(Paths.ImplementationFilePath, code);

        private void CreateSolutionFile() =>
            CreateFile(Paths.SolutionFilePath,$"{{\"track\":\"{Track}\",\"exercise\":\"{Exercise}\"}}");

        private static void CreateFile(string path, string contents) =>
            File.WriteAllText(path, contents);
    }
}