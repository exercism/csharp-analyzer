using System;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.CodeAnalysis;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    internal class CompilesWithoutErrorsRule : Rule
    {
        public override Task<Diagnostic[]> Verify(CompiledSolution compiledSolution)
        {
            if (HasCompileErrors(compiledSolution))
                return Task.FromResult(new[] {new Diagnostic("The code does not compile", DiagnosticLevel.Error)});

            return Task.FromResult(Array.Empty<Diagnostic>()); 
        }

        private static bool HasCompileErrors(CompiledSolution compiledSolution) 
            => compiledSolution.Compilation.GetDiagnostics().Any(IsError);

        private static bool IsError(Microsoft.CodeAnalysis.Diagnostic diagnostic) 
            => diagnostic.Severity == DiagnosticSeverity.Error;
    }
}