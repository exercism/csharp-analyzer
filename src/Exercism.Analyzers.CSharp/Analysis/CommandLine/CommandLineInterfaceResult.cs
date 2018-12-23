namespace Exercism.Analyzers.CSharp.Analysis.CommandLine
{
    public readonly struct CommandLineInterfaceResult
    {
        public readonly string Output;
        public readonly string Error;
        public readonly int ExitCode;

        public CommandLineInterfaceResult(string output, string error, int exitCode) =>
            (Output, Error, ExitCode) = (output, error, exitCode);
    }
}