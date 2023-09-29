using System;

public static class ReverseString
{
    public static string Reverse(string input)
    {
        Span<char> chars = stackalloc char[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            chars[input.Length - 1 - i] = input[i];
        }
        return new string(chars);
    }
}