namespace Exercism.Analyzers.CSharp.Analyzers.TwoFer
{
    public enum TwoFerValueType
    {
        Unknown,
        DefaultValue,
        NullCheck,
        NullCoalescingOperator,
        IsNullOrEmptyCheck,
        IsNullOrWhiteSpaceCheck
    }
}