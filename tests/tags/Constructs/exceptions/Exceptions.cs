using System;

public class CustomException : Exception
{
    public CustomException()
    {
    }
}

public static class Program
{
    public static void Exceptions()
    {
        try
        {
            throw new CustomException();
        }
        catch (Exception ex) when (ex.Message.Contains("404"))
        {
        }
        finally
        {
        }
    }
}