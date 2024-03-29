using System;
using System.Numerics;

public static class Grains
{
    public static ulong Square(int n)
    {
        switch (n)
        {
            case int num when num > 0 && num < 65: return (ulong)1 << (num - 1);
            default: throw new ArgumentOutOfRangeException("n must be 1 through 64");
        }
    }

    public static ulong Total()
    {
        return (ulong)((BigInteger.One << 64) - 1);
    }
}
