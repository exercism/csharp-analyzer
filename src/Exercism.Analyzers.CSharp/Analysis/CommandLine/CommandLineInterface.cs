using System.Diagnostics;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.CommandLine
{
    public abstract class CommandLineInterface
    {
        private readonly string _fileName;

        protected CommandLineInterface(string fileName) => _fileName = fileName;
        
        protected async Task<string> Run(string arguments)
        {
            using (var downloadProcess = new Process {StartInfo = CreateStartInfo(arguments)})
            {
                downloadProcess.Start();
                downloadProcess.WaitForExit();

                return await downloadProcess.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            }
        }
        
        private ProcessStartInfo CreateStartInfo(string arguments) 
            => new ProcessStartInfo
                {
                    FileName = _fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
    }
}