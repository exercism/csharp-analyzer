using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    public class TestSolution
    {
        private readonly string _name;
        
        public string Exercise { get; }
        public string Directory { get; }

        public TestSolution(string exercise, string name)
        {
            Exercise = exercise;
            _name = name;
            Directory = Path.Combine("solutions", exercise);
        }

        public void CreateFiles(string code)
        {
            CreateDirectory();
            CreateImplementationFile(code);
        }

        private void CreateDirectory()
        {
            if (System.IO.Directory.Exists(Directory))
                System.IO.Directory.Delete(Directory, recursive: true);

            System.IO.Directory.CreateDirectory(Directory);
        }

        private void CreateImplementationFile(string code) =>
            CreateFile($"{_name}.cs", code);

        private void CreateFile(string fileName, string contents) =>
            File.WriteAllText(Path.Combine(Directory, fileName), contents);
    }
}