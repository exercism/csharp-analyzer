using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using NullMessageSink = Xunit.Sdk.NullMessageSink;

namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    internal class AllTestsPassRule : Rule
    {
        private static readonly ISourceInformationProvider SourceInformationProvider = new NullSourceInformationProvider();
        private static readonly IMessageSink DiagnosticMessageSink = new NullMessageSink();
        private static readonly IMessageSink ExecutionMessageSink = new NullMessageSink();
        
        public override async Task<Diagnostic[]> Verify(CompiledSolution compiledSolution)
        {
            var testRunSummary = await RunTests(compiledSolution);
            
            if (testRunSummary.Skipped > 0)
                throw new InvalidOperationException("Test suite contains skipped tests");

            if (testRunSummary.Failed > 0)
                return new[] {new Diagnostic("Not all tests pass", DiagnosticLevel.Error)};

            return Array.Empty<Diagnostic>();
        }

        private static Task<RunSummary> RunTests(CompiledSolution compiledSolution)
        {
            using (var assemblyRunner = CreateTestAssemblyRunner(GetAssemblyInfo(compiledSolution)))
                return assemblyRunner.RunAsync();
        }

        private static XunitTestAssemblyRunner CreateTestAssemblyRunner(IReflectionAssemblyInfo assemblyInfo) 
            => new XunitTestAssemblyRunner(
                    new TestAssembly(assemblyInfo),
                    GetTestCases(assemblyInfo),
                    DiagnosticMessageSink,
                    ExecutionMessageSink,
                    TestFrameworkOptions.ForExecution());

        private static IEnumerable<IXunitTestCase> GetTestCases(IAssemblyInfo assemblyInfo)
        {
            using (var discoverySink = new TestDiscoverySink())
            using (var discoverer = new XunitTestFrameworkDiscoverer(assemblyInfo, SourceInformationProvider, DiagnosticMessageSink))
            {
                discoverer.Find(false, discoverySink, TestFrameworkOptions.ForDiscovery());
                discoverySink.Finished.WaitOne();

                return discoverySink.TestCases.Cast<IXunitTestCase>();
            }
        }

        private static IReflectionAssemblyInfo GetAssemblyInfo(CompiledSolution compiledSolution) 
            => Reflector.Wrap(LoadAssembly(compiledSolution));

        private static Assembly LoadAssembly(CompiledSolution compiledSolution)
        {
            using (var stream = new MemoryStream())
            {
                compiledSolution.Compilation.Emit(stream);
                return Assembly.Load(stream.ToArray());    
            }
        }
    }
}