using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class MissingAnalyzerTests : AnalyzerTests
    {
        public MissingAnalyzerTests() : base("missing", "Missing")
        {
        }
        
        [Fact]
        public void ReferToMentorWhenNoAnalyzerHasBeenImplementedForExercise() =>
            ShouldBeReferredToMentor(code: string.Empty);
    }
}