public static class Leap
{
    public static bool IsLeapYear(int year)
    {
        return (year % 4, year % 100, year % 400) switch
        {
            (_, _, 0) => true,
            (_, 0, _) => false,
            (0, _, _) => true,
            _ => false,
        };
    }
}
