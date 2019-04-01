using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        var you = string.IsNullOrWhiteSpace(input) ? "you" : input;
        return "One for " + you + ", one for me.";
    }
}