using System;

namespace Exercism.Analyzers.CSharp.IntegrationTests
{
    public static class Options
    {
        public static bool UpdateAnalysis =>
            GetBooleanEnvironmentVariable("UPDATE_ANALYSIS");

        public static bool UpdateComments =>
            GetBooleanEnvironmentVariable("UPDATE_COMMENTS");

        private static bool GetBooleanEnvironmentVariable(string name) =>
            bool.TryParse(Environment.GetEnvironmentVariable(name), out var enabled) && enabled;
    }
}