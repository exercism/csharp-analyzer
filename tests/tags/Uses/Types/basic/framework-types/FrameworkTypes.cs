using System;

public static class Types
{
    public static void Integrals()
    {
        sbyte explicitSbyte = 0;
        byte explicitByte = 1;
        short explicitShort = 2;
        ushort explicitUshort = 3;
        int explicitInt = 4;
        uint explicitUint = 5;
        long explicitLong = 6;
        uint explicitUlong = 7;
        nint explicitNint = 8;
        nuint explicitNuint = 9;
    }
    
    public static void FloatingPoint()
    {
        float explicitFloat = 0.0f;
        double explicitDouble = 1.0;
        decimal explicitDecimal = 2.0m;
    }

    public static void Other()
    {
        string explicitString = "";
        DateTime dateTime = new DateTime(1);
    }
}