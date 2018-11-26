using System;
using System.IO;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Cli
{
    internal static class ExercismCli
    {   
        public static async Task<DirectoryInfo> Download(string id)
        {
            var downloadedToDirectory = await ProcessRunner.Run(GetCliPath(), GetCliArguments(id));
            return new DirectoryInfo(downloadedToDirectory.Trim());
        }
        
        private static string GetCliPath()
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

        private static string GetCliArguments(string id) => $"download -u {id}";
    }
}