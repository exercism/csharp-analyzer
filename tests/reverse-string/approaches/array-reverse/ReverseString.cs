using System;

public static class ReverseString
{
    public static string Reverse(string input)
    {
        var chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}
