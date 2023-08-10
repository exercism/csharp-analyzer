public static class Leap
{
    public static bool IsLeapYear(long year)
    {
        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
    }
}