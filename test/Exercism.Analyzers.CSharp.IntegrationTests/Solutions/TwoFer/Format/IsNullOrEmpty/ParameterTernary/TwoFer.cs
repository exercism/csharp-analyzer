using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        input = string.IsNullOrEmpty(input) ? "you" : input;

        return string.Format("One for {0}, one for me.", input);
    }
}