using System;

namespace Library.GoogleMap
{
    static class StringExtensions
    {
        public static bool HasValue(this string source)
        {
            return !String.IsNullOrWhiteSpace(source);
        }
    }
}