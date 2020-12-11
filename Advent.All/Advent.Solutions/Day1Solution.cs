using System;
using System.IO;
using Advent.Solutions.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Advent.Solutions
{
    class Day1Solution : IAdventSolution
    {
        public int ProblemNumber => 1;

        public void Execute(StreamReader consoleReader, StreamWriter consoleWriter)
        {
            var input = GetProblemInput();
            var stopwatch = Stopwatch.StartNew();
            var solutions = new[]
            {
                FindSolutionOrNull(input, 2, 2020),
                FindSolutionOrNull(input, 3, 2020)
            };
            stopwatch.Stop();
            consoleWriter.WriteLine($"Measured {stopwatch.ElapsedMilliseconds} ms");
            foreach (var s in solutions)
                WriteSolution(s, consoleWriter);
        }

        private IReadOnlyList<int> GetProblemInput()
            => InputUtils.SplitLines(Resources.Day1Input)
            .Select( s => int.Parse(s) )
            .ToList();



        private Solution FindSolutionOrNull(IEnumerable<int> numbers, int count, int sum)
        {
            return numbers.CombinationsWithoutRepetition(count)
                .Select(Solution.Create)
                .FirstOrDefault(s => s.Sum == sum);
        }

        private void WriteSolution( Solution s, StreamWriter writer )
        {
            writer.WriteLine($"{string.Join(" * ", s.Factors)} => {s.Product}");
        }

        private class Solution
        {
            public IReadOnlyList<int> Factors { get; }
            public int Product { get; }
            public int Sum { get; }

            private Solution(IReadOnlyList<int> factors, int product, int sum)
            {
                Factors = factors.ToList();
                Product = product;
                Sum = sum;
            }

            public static Solution Create(IEnumerable<int> factors)
            {
                var factorsList = factors.ToList();
                return new Solution(
                    factorsList,
                    factorsList.Aggregate(1, (a, b) => a * b),
                    factorsList.Sum()
                );
            }

        }
    }
}
