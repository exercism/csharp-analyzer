using System;
using System.IO;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.CommandLine
{
    public class ExercismCommandLineInterface : CommandLineProcess
    {
        public ExercismCommandLineInterface() : base(GetFileName())
        {
        }

        public virtual async Task<DirectoryInfo> Download(string id)
        {
            var output = await Run(GetArguments(id));
            return new DirectoryInfo(output.Trim());
        }
        
        private static string GetFileName()
            => Path.Combine("binaries", $"{GetOperatingSystem()}-{GetArchitecture()}", "exercism");
        
        private static string GetOperatingSystem()
        {
            if (OperatingSystem.IsWindows())
                return "win";
            if (OperatingSystem.IsLinux())
                return "linux";
            if (OperatingSystem.IsMacOS())
                return "osx";
            
            throw new InvalidOperationException("Unsupported operating system");
        }

        private static string GetArchitecture()
        {
            if (Architecture.IsX86())
                return "x86";
            if (Architecture.IsX64())
                return "x64";

            throw new InvalidOperationException("Unsupported architecture system");
        }

        private static string GetArguments(string id) => $"download -u {id}";
    }
}