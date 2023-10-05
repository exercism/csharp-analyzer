using System;

public static class Leap
{
    public static bool IsLeapYear(int year) => (new DateTime(year, 2, 28)).AddDays(1.0).Day == 29;
}
