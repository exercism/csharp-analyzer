﻿namespace DefaultNamespace;

public static class Program
{
    public static void Main()
    {
        var i = 0;
        i += 1;
        i -= 2;
        i *= 3;
        i /= 4;
        i %= 5;
        i <<= 6;
        i >>= 7;
        i |= 8;
        i &= 9;
        i ^= 10;

        string j = null;
        j ??= "a";
    }
}
