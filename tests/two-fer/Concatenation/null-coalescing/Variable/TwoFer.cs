    using System;
    
    public static class TwoFer
    {
        public static string Speak(string input = null)
        {
            var nameOrYou = input ?? "you";
            return "One for " + nameOrYou + ", one for me.";
        }
    }