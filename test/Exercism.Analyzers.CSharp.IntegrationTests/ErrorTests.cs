using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ErrorTests
    {
        [Fact]
        public void ReturnErrorCodeWhenTrackIsNotCSharp()
        {
            var rubyTestSolution = new TestSolution("leap", "ruby");
            var analysisRun = TestSolutionAnalyzer.Run(rubyTestSolution, string.Empty);
            
            Assert.False(analysisRun.Success);
            Assert.False(analysisRun.Approved);
            Assert.False(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Messages);
        }
    }
}