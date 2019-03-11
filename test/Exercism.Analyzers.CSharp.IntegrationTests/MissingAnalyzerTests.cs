using Exercism.Analyzers.CSharp.IntegrationTests.Helpers;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class MissingAnalyzerTests
    {
        [Fact]
        public void ReferToMentorWhenNoAnalyzerHasBeenImplementedForExercise()
        {
            var analysisRun = TestSolutionAnalyzer.Run("missing", "Missing", string.Empty);

            Assert.True(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Comments);
        }
    }
}