using System;
using System.Linq;
using System.Collections.Generic;

namespace Quoridor.AI.Extensions
{
    public static class EnumerableExtensions
    {
        public static T MaxBy<T, U>(this IEnumerable<T> items, Func<T, U> fun)
            where U : IComparable<U>
        {
            return items.Aggregate((max, x) => fun(x).CompareTo(fun(max)) > 0 ? x : max);
        }

        public static T MinBy<T, U>(this IEnumerable<T> items, Func<T, U> fun)
            where U : IComparable<U>
        {
            return items.Aggregate((min, x) => fun(x).CompareTo(fun(min)) < 0 ? x : min);
        }

    }
}
