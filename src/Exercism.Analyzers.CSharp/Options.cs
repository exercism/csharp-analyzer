using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Exercism.Analyzers.CSharp
{
    public static class Options
    {
        public static string GetDirectory(IEnumerable<string> args)
        {
            var directory = args.FirstOrDefault();
            
            if (directory == null)
                Log.Error("Please specify a directory to analyze.");
            
            return directory;
        }
    }
}