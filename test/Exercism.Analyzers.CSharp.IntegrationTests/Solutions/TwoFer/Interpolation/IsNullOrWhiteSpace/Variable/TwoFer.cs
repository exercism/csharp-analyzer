using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        String name = string.IsNullOrWhiteSpace(input) ? "you" : input;
        return $"One for {name}, one for me.";
    }
}