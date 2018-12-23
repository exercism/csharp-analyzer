using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Exercism.Analyzers.CSharp.Analysis.CommandLine
{
    public abstract class CommandLineInterface
    {
        private readonly string _fileName;
        private readonly ILogger _logger;

        protected CommandLineInterface(string fileName, ILogger logger) =>
            (_fileName, _logger) = (fileName, logger);
        
        protected async Task<CommandLineInterfaceResult> Run(string arguments)
        {
            using (var process = new Process {StartInfo = CreateStartInfo(arguments)})
            {
                _logger.LogInformation("Executing CLI command '{File}' with arguments '{Arguments}'",
                    process.StartInfo.FileName, process.StartInfo.Arguments);
                
                process.Start();
                process.WaitForExit();
                
                _logger.LogInformation("Executed CLI command '{File}' with arguments '{Arguments}'",
                    process.StartInfo.FileName, process.StartInfo.Arguments);
                
                var output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
                var error = await process.StandardError.ReadToEndAsync().ConfigureAwait(false);
                
                if (process.ExitCode == 0)
                    _logger.LogInformation("Output of executed CLI command: '{Output}'", output);
                else
                    _logger.LogError("Error output of executed CLI command: '{Error}'", error);

                return new CommandLineInterfaceResult(output, error, process.ExitCode);
            }
        }
        
        private ProcessStartInfo CreateStartInfo(string arguments) =>
            new ProcessStartInfo
            {
                FileName = _fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
    }
}