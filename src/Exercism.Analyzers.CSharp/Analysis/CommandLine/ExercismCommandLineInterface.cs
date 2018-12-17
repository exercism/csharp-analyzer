using System;
using System.IO;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.CommandLine
{
    public class ExercismCommandLineInterface : CommandLineInterface
    {
        public ExercismCommandLineInterface() : base(GetFileName())
        {
        }
        
        private static string GetFileName()
            => Path.Combine("binaries", $"{GetOperatingSystem()}-{GetArchitecture()}", "exercism");
        
        private static string GetOperatingSystem()
        {
            if (OperatingSystem.IsWindows())
                return "win";
            if (OperatingSystem.IsLinux())
                return "linux";
            if (OperatingSystem.IsMacOs())
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

        public virtual async Task<DirectoryInfo> Download(string id)
        {
            var output = await Run(GetArguments(id)).ConfigureAwait(false);
            return new DirectoryInfo(output.Trim());
        }

        private static string GetArguments(string id) => $"download -u {id}";
    }
}