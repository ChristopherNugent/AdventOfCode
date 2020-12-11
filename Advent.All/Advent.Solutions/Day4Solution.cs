using System;
using System.Collections.Generic;
using System.IO;
using Advent.Solutions.Interfaces;
using System.Text.RegularExpressions;
using System.Linq;

namespace Advent.Solutions
{
    internal class Day4Solution : IAdventSolution
    {
        public int ProblemNumber => 4;

        public void Execute(StreamReader consoleReader, StreamWriter writer)
        {
            var passports = GetProblemInput();

            writer.WriteLine($"Total passports loaded: {passports.Count}");
            var validCount = passports.Count(p => p.IsValid(RequiredFields));
            writer.WriteLine($"Valid passports by field presence: {validCount}");

            validCount = passports.Count(p => p.IsValid(RequiredFieldsWithRules));
            writer.WriteLine($"Valid passports by rules: {validCount}");
        }

        private string[] RequiredFields { get; } = new[]
            { "ecl","pid","eyr","hcl","byr","iyr","hgt"};

        private IReadOnlyDictionary<string, Func<string, bool>> RequiredFieldsWithRules { get; }
            = new Dictionary<string, Func<string, bool>>()
            {
                {"byr", IsInRange(1920, 2002) },
                {"iyr", IsInRange(2010, 2020) },
                {"eyr", IsInRange(2020, 2030) },
                {"hgt", AnyOf(IsInRangeWithSuffix("cm", 150, 193), 
                              IsInRangeWithSuffix("in", 59, 96)) },
                {"hcl", new Regex(@"^#[0-9a-f]{6}$").IsMatch },
                {"ecl", new Regex(@"^(amb)|(blu)|(brn)|(gry)|(grn)|(hzl)|(oth)$").IsMatch },
                {"pid", new Regex(@"^\d{9}$").IsMatch }
            };

        private static Func<string, bool> IsInRange( int min, int max )
        {
            return (string s) => int.TryParse(s, out var res) && res >= min && res <= max;
        }

        private static Func<string, bool> IsInRangeWithSuffix( string suffix, int min, int max )
        {
            return (string s) => s.EndsWith(suffix) 
                && IsInRange(min, max)(s.Substring(0, s.Length - suffix.Length));
        }

        private static Func<string, bool> AnyOf( params Func<string, bool>[] funcs )
        {
            return (string s) => funcs.Any(f => f(s));
        }

        private List<Passport> GetProblemInput()
        {
            var passportSplitRegex = new Regex(@"(\r?\n){2,}");
            var passportStrings = passportSplitRegex.Split(Resources.Day4Input);
            var passports = passportStrings.Select(Passport.Parse);
            return passports.ToList(); ;
        }

        private class Passport
        {
            public IReadOnlyDictionary<string, string> Fields { get; }

            public Passport(IReadOnlyDictionary<string, string> entries)
            {
                Fields = entries;
            }

            public bool IsValid(IEnumerable<string> requiredFields )
            {
                return requiredFields.All(Fields.ContainsKey);
            }

            public bool IsValid(IReadOnlyDictionary<string, Func<string, bool>> requiredFields)
            {
                return requiredFields.All(field => Fields.ContainsKey(field.Key)
                    && field.Value(Fields[field.Key]));
            }

            private static readonly Regex FieldSplitRegex = new Regex(@"\s+"); 
            public static Passport Parse( string entryText )
            {
                var fields = FieldSplitRegex.Split(entryText).Where( s => !string.IsNullOrWhiteSpace(s));
                var fieldDict = fields.Select(ParseField).ToDictionary(f => f.key, f => f.value);
                return new Passport(fieldDict);
            }

            private static (string key, string value) ParseField( string fieldText )
            {
                var field = fieldText.Split(":");
                return (field[0], field[1]);
            }
        }
    }
}
