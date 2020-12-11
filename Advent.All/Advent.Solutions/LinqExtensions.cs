using System.Collections.Generic;
using System.Linq;
using System;

namespace Advent.Solutions
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> CombinationsWithoutRepetition<T>(
            this IEnumerable<T> items,
            int ofLength)
        {
            return (ofLength == 1) ?
                items.Select(item => Enumerable.Repeat(item, 1)) :
                items.SelectMany((item, i) => items.Skip(i + 1)
                                                   .CombinationsWithoutRepetition(ofLength - 1)
                                                   .Select(result => result.Prepend(item)));
        }

        public static IEnumerable<IEnumerable<T>> CombinationsWithoutRepetition<T>(
            this IEnumerable<T> items,
            int ofLength,
            int upToLength)
        {
            return Enumerable.Range(ofLength, Math.Max(0, upToLength - ofLength + 1))
                             .SelectMany(len => items.CombinationsWithoutRepetition(ofLength: len));
        }
    }
}