using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolution
    {
        private readonly string _exercise;
        private readonly string _name;
        private readonly string _track;
        
        public string Directory { get; }

        public TestSolution(string exercise, string name, string track = "csharp")
        {
            _exercise = exercise;
            _name = name;
            _track = track;
            Directory = Path.Combine("solutions", track, exercise);
        }

        public void CreateFiles(string code)
        {
            CreateDirectory();
            CreateImplementationFile(code);
            CreateSolutionFile();
        }

        private void CreateDirectory()
        {
            if (System.IO.Directory.Exists(Directory))
                System.IO.Directory.Delete(Directory, recursive: true);

            System.IO.Directory.CreateDirectory(Directory);
        }

        private void CreateImplementationFile(string code) =>
            CreateFile($"{_name}.cs", code);

        private void CreateSolutionFile() =>
            CreateFile(".solution.json",$"{{\"track\":\"{_track}\",\"exercise\":\"{_exercise}\"}}");

        private void CreateFile(string fileName, string contents) =>
            File.WriteAllText(Path.Combine(Directory, fileName), contents);
    }
}