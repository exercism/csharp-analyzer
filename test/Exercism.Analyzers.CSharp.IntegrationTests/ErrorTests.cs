using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class ErrorTests
    {
        [Fact(Skip = "Convert to syntax error test")]
        public void ReturnErrorCodeWhenTrackIsNotCSharp()
        {
            var rubyTestSolution = new TestSolution("leap", "Leap");
            var analysisRun = TestSolutionAnalyzer.Run(rubyTestSolution, string.Empty);

            Assert.True(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Comments);
        }
    }
}