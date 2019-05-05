namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    public enum TwoFerError
    {
        None,
        UsesOverloads,
        MissingSpeakMethod,
        InvalidSpeakMethod,
        UsesDuplicateString,
        NoDefaultValue,
        InvalidDefaultValue,
        UsesStringReplace,
        UsesStringJoin,
        UsesStringConcat
    }
}