namespace Exercism.Analyzers.CSharp.Analyzers.Gigasecond
{
    internal enum AddSecondsArgumentValueType
    {
        Unknown,
        DigitsWithSeparator,
        DigitsWithoutSeparator,
        ScientificNotation,
        MathPow
    }
}