using System.Linq;

namespace Exercism.Analyzers.CSharp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Logging.Configure();

            var directory = args.FirstOrDefault();
            return Analyzer.Analyze(directory);
        }
    }
}