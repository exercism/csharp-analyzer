using System; // for DateTime

public static bool IsLeapYear(int year) => (new DateTime(year, 2, 28)).AddDays(1.0).Day == 29;
