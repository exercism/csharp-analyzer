using System.Diagnostics;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Cli
{
    internal static class ProcessRunner
    {
        public static Task<string> Run(string fileName, string arguments)
        {
            using (var downloadProcess = new Process {StartInfo = CreateStartInfo(fileName, arguments)})
            {
                downloadProcess.Start();
                downloadProcess.WaitForExit();

                return downloadProcess.StandardOutput.ReadToEndAsync();
            }
        }
        
        private static ProcessStartInfo CreateStartInfo(string fileName, string arguments) => 
            new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
    }
}