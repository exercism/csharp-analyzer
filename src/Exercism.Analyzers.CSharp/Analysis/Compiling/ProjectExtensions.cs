using System.Linq;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Compiling
{
    internal static class ProjectExtensions
    {
        public static Document GetDocument(this Project project, string name)
            => project.Documents.FirstOrDefault(document => document.Name == name);
    }
}