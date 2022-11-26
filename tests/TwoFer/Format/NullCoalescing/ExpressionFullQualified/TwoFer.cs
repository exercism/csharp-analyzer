using System;

public static class TwoFer
{
    public static string Speak(string input = null) =>
        System.String.Format("One for {0}, one for me.", input ?? "you");
}