using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        if (input == null)
        {
            return "One for you, one for me.";
        }
        
        return String.Format("One for {0}, one for me.", input);
    }
}