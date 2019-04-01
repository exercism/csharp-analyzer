    using System;
    
    public static class TwoFer
    {
        public static string Name(string input = null)
        {
            var nameOrYou = input ?? "you";
            return "One for " + nameOrYou + ", one for me.";
        }
    }