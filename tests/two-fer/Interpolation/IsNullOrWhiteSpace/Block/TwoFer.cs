using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        return $"One for {(string.IsNullOrWhiteSpace(input) ? "you" : input)}, one for me.";
    }
}