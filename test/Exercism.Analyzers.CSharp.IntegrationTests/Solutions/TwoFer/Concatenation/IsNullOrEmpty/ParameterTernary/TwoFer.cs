using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        input = string.IsNullOrEmpty(input) ? "you" : input;
        return "One for " + input + ", one for me.";
    }
}