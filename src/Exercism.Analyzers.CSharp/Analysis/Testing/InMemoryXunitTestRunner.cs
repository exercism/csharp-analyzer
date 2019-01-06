using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Compiling;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Exercism.Analyzers.CSharp.Analysis.Testing
{
    internal static class InMemoryXunitTestRunner
    {
        private static readonly ISourceInformationProvider SourceInformationProvider = new NullSourceInformationProvider();
        private static readonly IMessageSink DiagnosticMessageSink = new Xunit.Sdk.NullMessageSink();
        private static readonly IMessageSink ExecutionMessageSink = new Xunit.Sdk.NullMessageSink();

        public static async Task<RunSummary> RunAllTests(Microsoft.CodeAnalysis.Compilation compilation)
        {
            var compilationWithAllTestsEnabled = compilation.EnableAllTests();
            var assemblyInfo = GetAssemblyInfo(compilationWithAllTestsEnabled);

            using (var assemblyRunner = CreateTestAssemblyRunner(assemblyInfo))
                return await assemblyRunner.RunAsync();
        }

        private static IReflectionAssemblyInfo GetAssemblyInfo(Microsoft.CodeAnalysis.Compilation compilation) =>
            Reflector.Wrap(compilation.GetAssembly());

        private static XunitTestAssemblyRunner CreateTestAssemblyRunner(IAssemblyInfo assemblyInfo) =>
            new XunitTestAssemblyRunner(
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
    }
}