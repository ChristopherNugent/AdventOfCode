using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Advent.Solutions
{
    internal static class InputUtils
    {
        public static string[] SplitIntoLineGroups( string input )
        {
            return new Regex(@"(\r?\n){2,}").Split(input);
        }

        public static string RemoveWhitespace( string input )
        {
            return new Regex(@"\s+").Replace(input, "");
        }

        public static string[] SplitLines( string input )
        {
            return new Regex(@"\r?\n").Split(input)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToArray();
        }
    }
}
