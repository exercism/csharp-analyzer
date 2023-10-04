using System;
using System.Collections.Generic;
using System.Linq;

public static class Series
{
    public static IEnumerable<string> Slices(string input, int length)
    {
        if (length < 1 || length > input.Length)
            throw new ArgumentException("Invalid length");

        return Enumerable.Range(0, input.Length - length + 1)
            .Select(i => input.Substring(i, length));
    }
}
