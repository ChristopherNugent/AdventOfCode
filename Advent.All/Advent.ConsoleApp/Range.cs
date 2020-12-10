using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advent.ConsoleApp
{
    internal class Range
    {
        public int Min { get; }
        public int Max { get; }

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"{Min}-{Max}";
        }

        public static IEnumerable<Range> CreateRanges( IEnumerable<int> values )
        {
            var orderedValues = values.OrderBy(i => i);

            var min = orderedValues.First();
            var max = min;
            foreach( var i in orderedValues.Skip(1))
            {
                if (i == max + 1)
                    max++;
                else
                {
                    yield return new Range(min, max);
                    min = max = i;
                }
            }
            yield return new Range(min, max);
        }
    }
}
