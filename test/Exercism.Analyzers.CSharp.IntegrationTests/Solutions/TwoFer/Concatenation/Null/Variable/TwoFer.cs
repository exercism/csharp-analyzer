using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        var you = input == null ? "you" : input;
        return string.Format("One for {0}, one for me.", you);
    }
}