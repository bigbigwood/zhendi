using System;
using System.Collections.Generic;

namespace Rld.Acs.Unility.Extension
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) return;
            if (action == null) return;

            foreach (T t in source)
            {
                action(t);
            }
        }


        public static List<T> FindAll<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            if (source == null) return new List<T>();
            if (predicate == null) return new List<T>();

            return new List<T>(source).FindAll(predicate);
        }
    }
}
