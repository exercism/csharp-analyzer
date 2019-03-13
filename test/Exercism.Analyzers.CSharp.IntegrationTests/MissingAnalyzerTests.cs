using System.Threading.Tasks;
using Xunit;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public class MissingAnalyzerTests : AnalyzerTests
    {
        public MissingAnalyzerTests() : base("missing", "Missing")
        {
        }
        
        [Fact]
        public async Task ReferToMentorWhenNoAnalyzerHasBeenImplementedForExercise() =>
            await ShouldBeReferredToMentor(code: string.Empty);
    }
}