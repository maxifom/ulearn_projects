using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    /// <summary>
    /// Анализатор пар
    /// </summary>
    public static class Analysis
    {
        /// <summary>
        /// Находит индекс максимального
        /// периода между датами
        /// </summary>
        /// <param name="data">
        /// Даты для поиска
        /// </param>
        /// <returns>
        /// Индекс максимального
        /// периода между датами
        /// </returns>
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            return new MaxPauseFinder().Analyze(data);
        }

        /// <summary>
        /// Находит среднюю разницу
        /// между датами
        /// </summary>
        /// <param name="data">
        /// Даты для поиска среднего
        /// </param>
        /// <returns>
        /// Среднюю разницу между датами
        /// </returns>
        public static double FindAverageRelativeDifference(params double[] data)
        {
            return new AverageDifferenceFinder().Analyze(data);
        }
    }

    /// <summary>
    /// Расширения для анализатора
    /// </summary>
    public static class AnalysisExtensions
    {
        /// <summary>
        /// Преобразует перечислимый тип
        /// в перечислимый картеж
        /// </summary>
        /// <typeparam name="T">
        /// Тип данных
        /// </typeparam>
        /// <param name="data">
        /// Данные для преобразования
        /// </param>
        /// <returns>
        /// Перечислимый картеж данных
        /// </returns>
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> data)
            where T : struct
        {
            T? last_el = null;
            foreach (var el in data)
            {
                if (!last_el.Equals(null))
                {
                    yield return new Tuple<T, T>((T)last_el, el);
                }
                last_el = el;
            }
        }

        /// <summary>
        /// Находит индекс 
        /// максимального элемента
        /// </summary>
        /// <typeparam name="T">
        /// Тип данных
        /// </typeparam>
        /// <param name="data">
        /// Данные для поиска
        /// </param>
        /// <returns>
        /// Индекс максимального элемента
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Вызывается при отсутствии элементов
        /// в данных
        /// </exception>
        public static int MaxIndex<T>(this IEnumerable<T> data)
            where T : IComparable
        {
            var maxValue = default(T);
            int indexOfMax = -1;
            int index = 0;
            foreach (var el in data)
            {
                if (el.CompareTo(maxValue) == 1)
                {
                    maxValue = el;
                    indexOfMax = index;
                }
                index++;
            }
            if (index == 0)
                throw new ArgumentException();
            return indexOfMax;
        }
    }
}
