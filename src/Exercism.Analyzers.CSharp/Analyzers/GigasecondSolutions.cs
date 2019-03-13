namespace Exercism.Analyzers.CSharp.Analyzers
{
    internal static class GigasecondSolutions
    {
        public const string AddSecondsWithScientificNotationInExpressionBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1e9);
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
        
        public const string AddSecondsWithMathPowInExpressionBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
            }";
        
        public const string AddSecondsWithMathPowInBlockBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate)
                {
                    return birthDate.AddSeconds(Math.Pow(10, 9));
                }
            }";
        
        public const string AddSecondsWithDigitsWithoutSeparatorInExpressionBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000);
            }";
        
        public const string AddSecondsWithDigitsWithoutSeparatorInBlockBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate)
                {
                    return birthDate.AddSeconds(1000000);
                }
            }";
        
        public const string AddInExpressionBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
            }";
        
        public const string AddInBlockBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate)
                {
                    return birthDate.Add(TimeSpan.FromSeconds(1000000000));
                }
            }";
        
        public const string PlusOperatorInExpressionBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate) => birthDate + TimeSpan.FromSeconds(1000000000);
            }";
        
        public const string PlusOperatorInBlockBody = @"
            using System;
            
            public static class Gigasecond
            {
                public static DateTime Add(DateTime birthDate)
                {
                    return birthDate + TimeSpan.FromSeconds(1000000000);
                }
            }";
    }
}