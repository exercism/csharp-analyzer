using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        var name = input == null ? "you" : input;
        return $"One for {name}, one for me.";
    }
}