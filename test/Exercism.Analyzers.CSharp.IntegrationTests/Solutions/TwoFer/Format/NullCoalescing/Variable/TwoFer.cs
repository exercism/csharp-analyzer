using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        var name = input ?? "you";
        return string.Format("One for {0}, one for me.", name);
    }
}