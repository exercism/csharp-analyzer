using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        return "One for " + (string.IsNullOrEmpty(input) ? "you" : input) + ", one for me.";
    }
}