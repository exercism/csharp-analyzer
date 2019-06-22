public static class Leap
{
    public static bool IsLeapYear(int year)
    {
        if (year % 4 == 0)
        {
            if (year % 100 != 0)
            {
                return true;
            }

            if (year % 400 == 0)
            {
                return true;
            }
        }

        return false;
    }
}