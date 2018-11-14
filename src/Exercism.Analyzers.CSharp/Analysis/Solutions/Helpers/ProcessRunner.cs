using System.Diagnostics;
using System.Threading.Tasks;

namespace Exercism.Analyzers.CSharp.Analysis.Solutions.Helpers
{
    internal static class ProcessRunner
    {
        public static Task<string> Run(string verb, string arguments)
        {
            using (var downloadProcess = new Process {StartInfo = CreateStartInfo(verb, arguments)})
            {
                downloadProcess.Start();
                downloadProcess.WaitForExit();

                return downloadProcess.StandardOutput.ReadToEndAsync();
            }
        }
        
        private static ProcessStartInfo CreateStartInfo(string verb, string arguments) => 
            new ProcessStartInfo
            {
                Verb = verb,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
    }
}