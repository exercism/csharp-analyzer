using System;

public static class TwoFer
{
    public static string Speak(string input = "you")
    {
        return string.Format("One for {0}, one for me.", input);
    }
}