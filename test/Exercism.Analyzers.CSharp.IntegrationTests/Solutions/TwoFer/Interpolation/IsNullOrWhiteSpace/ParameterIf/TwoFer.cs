using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            input = "you";
        }
        return $"One for {input}, one for me.";
    }
}