using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Solutions.Interfaces;
using System.IO;

namespace Advent.Solutions
{
    public class Day2Solution : IAdventSolution
    {
        public int ProblemNumber => 2;

        public void Execute(StreamReader consoleReader, StreamWriter writer)
        {
            var input = GetProblemInput().ToList();
            RunAndPrintInput(input, e => e.IsValid(), writer);
            RunAndPrintInput(input, e => e.IsSecondaryValid(), writer);
        }

        private void RunAndPrintInput<T>(
            IReadOnlyList<T> entries, 
            Func<T, bool> isValidFunc,
            StreamWriter writer )
        {
            var goodCount = entries.Count(isValidFunc);
            var badCount = entries.Count - goodCount;

            writer.WriteLine($"Valid passwords: {goodCount} Invalid passwords: {badCount}");
        }

        private IEnumerable<PasswordEntry> GetProblemInput()
            => Resources.Day2Input.Split("\r\n")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(PasswordEntry.Parse);

        private class PasswordEntry
        {
            public int Min { get; }
            public int Max { get; }
            public char Character { get; }
            public string Password { get; }

            public PasswordEntry(char character, int min, int max, string password)
            {
                Character = character;
                Min = min;
                Max = max;
                Password = password;
            }

            private static readonly Regex parserRegex
                = new Regex(@"(?<min>\d+)-(?<max>\d+) (?<char>.): (?<pass>\S+)");
            public static PasswordEntry Parse( string s )
            {
                var match = parserRegex.Match(s);
                return new PasswordEntry(
                    match.Groups["char"].Value[0],
                    int.Parse(match.Groups["min"].Value),
                    int.Parse(match.Groups["max"].Value),
                    match.Groups["pass"].Value);
            }

            public bool IsValid()
            {
                var letterCount = Password.Count(c => c == Character);
                return letterCount >= Min && letterCount <= Max;
            }

            public bool IsSecondaryValid()
            {
                return Password[Min - 1] == Character ^ Password[Max - 1] == Character;
            }
        }
    }
}
