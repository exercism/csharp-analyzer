using System;

public static class TwoFer
{
    public static string Speak()
    {
        return "One for you, one for me.";
    }

    public static string Speak(string input)
    {
        return "One for " + input + ", one for me.";
    }
}
