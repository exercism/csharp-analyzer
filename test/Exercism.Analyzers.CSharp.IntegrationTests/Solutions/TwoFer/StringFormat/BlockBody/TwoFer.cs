using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        var inputOrYou = input ?? "you";
        return string.Format("One for {0}, one for me.", inputOrYou);
    }
}