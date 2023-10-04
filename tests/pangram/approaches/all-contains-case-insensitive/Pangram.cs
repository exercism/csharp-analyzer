using System;
using System.Linq;

public static class Pangram
{
    private static readonly StringComparison xcase = StringComparison.CurrentCultureIgnoreCase;
    
    public static bool IsPangram(string input)
    {
        return "abcdefghijklmnopqrstuvwxyz".All(c => input.Contains(c, xcase));
    }
}
