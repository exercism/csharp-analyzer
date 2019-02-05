using System.IO;

namespace Exercism.Analyzers.CSharp.IntegrationTests.Helpers
{
    internal static class DirectoryInfoExtensions
    {
        public static void Recreate(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo.Exists)
                directoryInfo.Delete(recursive: true);

            directoryInfo.Create();
        }
    }
}