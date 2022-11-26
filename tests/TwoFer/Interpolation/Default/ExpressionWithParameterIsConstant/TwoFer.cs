using System;

public static class TwoFer
{
    private const string You = "you";
    public static string Speak(string input = You) =>
    $"One for {input}, one for me.";
}