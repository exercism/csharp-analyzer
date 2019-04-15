using System;

public static class TwoFer
{
    public static string Speak(string input = null) =>
        $"One for {(string.IsNullOrEmpty(input) ? "you" : input)}, one for me.";
}