using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.CommandLine
{
    public class ExercismCommandLineInterface : CommandLineInterface
    {
        private readonly ILogger<ExercismCommandLineInterface> _logger;

        public ExercismCommandLineInterface(ILogger<ExercismCommandLineInterface> logger) : base(GetFileName(), logger) =>
            _logger = logger;

        public virtual async Task<DirectoryInfo> Download(string id)
        {
            var arguments = GetArguments(id);

            _logger.LogInformation("Executing exercism CLI command for solution {ID}: {Command}", id, arguments);
            
            var output = await Run(arguments).ConfigureAwait(false);

            _logger.LogInformation("Executed exercism CLI command for solution {ID}", id);
            
            return new DirectoryInfo(output.Output.Trim());
        }

        private static string GetArguments(string id) => $"download -u {id}";
        
        private static string GetFileName() =>
            Path.Combine("binaries", $"{GetOperatingSystem()}-{GetArchitecture()}", "exercism");
        
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
    }
}