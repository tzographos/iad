using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection.Core.Helpers
{
    public static class StatisticsExtensions
    {
        public static double Variance<T>(this IEnumerable<T> values, Func<T, double> selector)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            double average = values.Average(selector);

            return values
                .Select(v => Math.Pow(selector(v) - average, 2))
                .Average();
        }


        /// <summary>
        /// Standard deviation of the population
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static double StandardDeviation<T>(this IEnumerable<T> values, Func<T, double> selector)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            return Math.Sqrt(values.Variance<T>(selector));
        }



    }
}
