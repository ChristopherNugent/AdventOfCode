using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Advent.Solutions.Interfaces;
using System.IO;
using System.Diagnostics;

namespace Advent.Solutions
{
    internal class Day7Solution : IAdventSolution
    {
        public int ProblemNumber => 7;
        private const string Gold = "shiny gold";

        private IReadOnlyList<BagRule> rules;

        public void Execute(StreamReader consoleReader, StreamWriter writer)
        {
            if (rules == null)
            {
                writer.WriteLine("Loading and parsing bag rules...");
                rules = LoadInput();
                writer.WriteLine($"Loaded {rules.Count} rules.");
            }
            else
            {
                writer.WriteLine("Reusing cached bag rules.");
            }
            var ruleDictionary = rules.ToDictionary(r => r.Color);
            var colors = rules.Select(r => r.Color);
            var colorsHoldingGolds = colors.Count(color => SeriesContains(color, ruleDictionary, Gold));
            writer.WriteLine($"Starting colors which hold a shiny gold: {colorsHoldingGolds}");

            var goldContentsCount = ExpandBags(Gold, ruleDictionary).Sum(kvp => kvp.Value);
            writer.WriteLine($"Number of bags within a shiny gold: {goldContentsCount}");
        }

        private List<BagRule> LoadInput()
            => InputUtils.SplitLines(Resources.Day7Input)
                .Select(BagRule.Parse)
                .ToList();

        private bool SeriesContains( 
            string startingBag,
            IReadOnlyDictionary<string, BagRule> ruleDictionary,
            string targetBag )
        {
            return ExpandBags(startingBag, ruleDictionary).ContainsKey(targetBag);
        }

        private Dictionary<string, int> ExpandBags(
            string startingBag, 
            IReadOnlyDictionary<string, BagRule> ruleDictionary,
            Func<IReadOnlyDictionary<string, int>, bool> continueWhile = null )
        {
            var exploredCounts = new Dictionary<string, int>();

            var unexploredCounts = new Dictionary<string, int>(ruleDictionary[startingBag].BagCounts);
            while (unexploredCounts.Any(kvp => kvp.Value > 0) 
                && (continueWhile?.Invoke(exploredCounts) ?? true) )
            {
                var nextGeneration = new Dictionary<string, int>();
                foreach(var kvp in unexploredCounts)
                {
                    AddToDictionary(
                        nextGeneration,
                        ruleDictionary[kvp.Key].BagCounts,
                        kvp.Value);
                }
                AddToDictionary(exploredCounts, unexploredCounts, 1);
                unexploredCounts = nextGeneration;
            }
            return exploredCounts;
        }

        public void AddToDictionary(
            Dictionary<string, int> first,
            IReadOnlyDictionary<string, int> second,
            int multiplier )
        {
            foreach( var (key, value) in second)
            {
                if(!first.ContainsKey(key))
                {
                    first[key] = multiplier * value;
                } else
                {
                    first[key] += multiplier * value;
                }
            }
        }

        [DebuggerDisplay("Rule for {Color}")]
        private class BagRule
        {
            /// <summary>
            /// The color of this bag.
            /// </summary>
            public string Color { get; }

            /// <summary>
            /// The number of bags contained by this bag, by color.
            /// </summary>
            public IReadOnlyDictionary<string, int> BagCounts { get; }

            private BagRule( string color, IReadOnlyDictionary<string, int> counts )
            {
                Color = color;
                BagCounts = counts;
            }

            private static Regex parseRegex;
            public static BagRule Parse( string rule )
            {
                if( parseRegex == null )
                {
                    
                    parseRegex = new Regex(
                        @"(?<thisBag>.+) bags contain (?<contents>((?<count>\d+) (?<color>[^.,]+) bags?[,.]\s?)+|(no other bags.))",
                        RegexOptions.Compiled );
                }
                var match = parseRegex.Match(rule);
                var thisBagColor = match.Groups["thisBag"].Value;
                var otherBagCounts = match.Groups["count"].Captures.Select(c => int.Parse(c.Value));
                var otherBagColors = match.Groups["color"].Captures.Select(c => c.Value);

                var countDictionary = otherBagColors
                    .Zip(otherBagCounts, (k, v) => (key: k, val: v))
                    .ToDictionary(p => p.key, p => p.val);

                return new BagRule(thisBagColor, countDictionary);
            }
        }
    }
}
