using System.Runtime.InteropServices;

namespace Exercism.Analyzers.CSharp.Analysis.Cli
{
    internal static class Architecture
    {
        public static bool IsX86() 
            => RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.X86;
        
        public static bool IsX64() 
            => RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.X64;
    }
}