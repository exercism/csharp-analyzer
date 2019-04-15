using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        var you = string.IsNullOrEmpty(input) ? "you" : input;
        return "One for " + you + ", one for me.";
    }
}