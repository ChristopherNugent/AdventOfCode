using System;
using System.IO;
using Advent.Solutions.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace Advent.Solutions
{
    internal class Day6Solution : IAdventSolution
    {
        public int ProblemNumber => 6;

        public void Execute(StreamReader consoleReader, StreamWriter writer)
        {
            var groups = InputUtils.SplitIntoLineGroups(Resources.Day6Input);

            var unionCounts = groups
                .Select(InputUtils.RemoveWhitespace)
                .Select(g => g.Distinct().Count());
            writer.WriteLine($"Sum of questions where anyone answers yes: {unionCounts.Sum()}");

            var disjointCounts = groups
                .Select(InputUtils.SplitLines)
                .Select(g => g.Aggregate(g.FirstOrDefault() ?? Enumerable.Empty<char>(),
                                         (agg, curr) => agg.Intersect(curr)))
                .Select(g => g.Count());

            writer.WriteLine($"Sum of questions where everyone answers yes: {disjointCounts.Sum()}");
        }
    }
}
