using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        if (input == null)
        {
            return $"One for you, one for me.";
        }
        else
        {
            return $"One for {input}, one for me.";   
        }
    }
}