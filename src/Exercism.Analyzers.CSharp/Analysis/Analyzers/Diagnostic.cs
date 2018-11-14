namespace Exercism.Analyzers.CSharp.Analysis.Analyzers
{
    public class Diagnostic
    {
        public Diagnostic(string text, DiagnosticLevel level) => (Text, Level) = (text, level);
        
        public string Text { get; }
        public DiagnosticLevel Level { get; } 
    }
}