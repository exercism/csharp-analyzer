using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ErrorTests
    {
        [Fact]
        public void ReturnErrorCodeWhenTrackIsNotCSharp()
        {
            var rubyTestSolution = new TestSolution("leap", "Leap", "ruby");
            var analysisRun = TestSolutionAnalyzer.Run(rubyTestSolution, string.Empty);
            
            Assert.False(analysisRun.Success);
            Assert.True(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Comments);
        }
    }
}