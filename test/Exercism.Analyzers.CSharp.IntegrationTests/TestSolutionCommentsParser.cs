using System.Linq;
using Newtonsoft.Json.Linq;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    internal static class TestSolutionCommentsParser
    {
        public static TestSolutionComment[] ParseComments(string analysis) =>
            JToken.Parse(analysis).ParseComments();

        private static TestSolutionComment[] ParseComments(this JToken jsonAnalysisResult) =>
            jsonAnalysisResult["comments"].Select(ParseComment).ToArray();

        private static TestSolutionComment ParseComment(JToken comment) =>
            new TestSolutionComment(comment.ParseCommentText(), comment.ParseCommentParameters());

        private static string ParseCommentText(this JToken comment) =>
            comment["comment"].ToString();

        private static TestSolutionCommentParameter[] ParseCommentParameters(this JToken comment) =>
            comment["params"].Children<JProperty>().Select(ParseCommentParameter).ToArray();

        private static TestSolutionCommentParameter ParseCommentParameter(this JProperty parameter) =>
            new TestSolutionCommentParameter(parameter.Name, parameter.Value.ToString());
    }
}