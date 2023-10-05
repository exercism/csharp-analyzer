using System;
using System.Reflection;

public static class Reflective
{
    public static void UsingGetType()
    {
        var x = 2.GetType();
    }
    
    public static void UsingTypeof()
    {
        var x = typeof(int);
    }
}