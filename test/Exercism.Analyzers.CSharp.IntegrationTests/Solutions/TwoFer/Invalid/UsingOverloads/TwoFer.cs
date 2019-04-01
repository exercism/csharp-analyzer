using System;

public static class TwoFer
{
    public static string Name()
    {
        return "One for you, one for me.";
    }

    public static string Name(string input)
    {
        return "One for " + input + ", one for me.";
    }
}
