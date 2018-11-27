namespace Exercism.Analyzers.CSharp.Analysis.Analyzers.Rules
{
    public readonly struct Diagnostic
    {
        public Diagnostic(string text, DiagnosticLevel level) => (Text, Level) = (text, level);

        public readonly string Text;
        public readonly DiagnosticLevel Level;
    }
}