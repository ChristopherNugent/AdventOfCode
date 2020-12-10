using System;
using System.IO;
using Advent.Solutions.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Solutions
{
    internal class Day3Solution : IAdventSolution
    {
        public int ProblemNumber => 3;

        public void Execute(StreamReader consoleReader, StreamWriter writer)
        {
            var map = GetProblemInput();
            var treeCount = YieldPath(map, 0, 0, 3, 1).Count(t => t == TileType.Tree);
            writer.WriteLine($"Number of trees: {treeCount}");

            var paths = new[]
            {
                (dx: 1, dy: 1),
                (dx: 3, dy: 1),
                (dx: 5, dy: 1),
                (dx: 7, dy: 1),
                (dx: 1, dy: 2)
            };

            var productOfPathTreeCounts = paths
                .Select(p => YieldPath(map, 0, 0, p.dx, p.dy).Count(t => t == TileType.Tree))
                .Aggregate(1L, (a, b) => a * b);

            writer.WriteLine($"Product of number of trees on paths: {productOfPathTreeCounts}");

        }

        private IEnumerable<TileType> YieldPath( Map map, int initX, int initY, int dx, int dy )
        {
            var x = initX;
            var y = initY;
            while (y < map.Height)
            {
                yield return map[x, y];
                x += dx;
                y += dy;
            }
        }

        private Map GetProblemInput()
            => Map.Parse(Resources.Day3Input);

        private enum TileType
        {
            Open,
            Tree
        }

        private class Map
        {
            private readonly IReadOnlyList<IReadOnlyList<TileType>> map;
            public int Height => map.Count;
            public int Width => map.Count > 0 ? map[0].Count : 0;

            public TileType this[int x, int y] => map[y][x % Width];

            private Map(IReadOnlyList<IReadOnlyList<TileType>> map)
            {
                if (!map.All(l => l.Count == map.First().Count))
                {
                    throw new ArgumentException("Map rows must all have the same length", nameof(map));
                }

                this.map = map;
            }

            public static Map Parse( string input )
            {
                var map = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(ParseRow)
                    .ToList();

                return new Map(map);
            }

            private static IReadOnlyList<TileType> ParseRow(string rowText)
                => rowText.Select(ParseTile).ToList();

            private static TileType ParseTile(char c)
                => c == '#' ? TileType.Tree : TileType.Open;

        }
    }
}
