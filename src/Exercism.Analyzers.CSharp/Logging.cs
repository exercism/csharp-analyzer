using Serilog;

namespace Exercism.Analyzers.CSharp
{
    internal static class Logging
    {
        public static void Configure() =>
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
    }
}