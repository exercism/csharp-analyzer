namespace Exercism.Analyzers.CSharp.Analyzers
{
    public static class GigasecondSolutions
    {
        public const string AddSecondsWithScientificNotation = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1e9);
            }";
        
        public const string AddSecondsWithMathPow = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
            }";
        
        public const string AddSecondsWithDigitsWithoutSeparator = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000);
            }";
        
        public const string AddSecondsWithScientificNotationInBlockBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate)
                {
                    return birthDate.AddSeconds(1e9);
                }
            }";
        
        public const string Add = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
            }";
        
        public const string PlusOperator = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate + TimeSpan.FromSeconds(1000000000);
            }";
    }
}