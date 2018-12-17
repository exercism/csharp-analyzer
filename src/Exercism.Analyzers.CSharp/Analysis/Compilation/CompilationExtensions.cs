using System.IO;
using System.Reflection;

namespace Exercism.Analyzers.CSharp.Analysis.Compilation
{
    internal static class CompilationExtensions
    {
        public static Assembly GetAssembly(this Microsoft.CodeAnalysis.Compilation compilation)
        {
            using (var memoryStream = new MemoryStream())
            {
                compilation.Emit(memoryStream);
                return Assembly.Load(memoryStream.ToArray());    
            }
        }
    }
}