using System;
using System.Collections.Generic;
using System.Text;

namespace AnomalyDetection.Core.Helpers
{
    public static class EnumerableExtensions
    {

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            //check for null OR empty
            return !enumerable?.GetEnumerator().MoveNext() ?? true;
        }
        

        /// <summary>
        /// Adds a range of objects in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection on which the action will be performed</param>
        /// <param name="range">The range of elements that are going to be added; if null or empty no action is taken</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            //check for null
            if (collection == null || range == null) return;
            //add elements of range in collection
            foreach (T e in range)
            {
                collection.Add(e);
            }
        }


    }
}
