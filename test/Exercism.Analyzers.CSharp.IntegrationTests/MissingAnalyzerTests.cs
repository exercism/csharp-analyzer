using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class MissingAnalyzerTests : AnalyzerTests
    {
        public MissingAnalyzerTests() : base("missing", "Missing")
        {
        }
        
        [Fact]
        public void ReferToMentorWhenNoAnalyzerHasBeenImplementedForExercise()
        {
            var analysisRun = Analyze(code: string.Empty);

            Assert.True(analysisRun.ReferToMentor);
            Assert.Empty(analysisRun.Comments);
        }
    }
}