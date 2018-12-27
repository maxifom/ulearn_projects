using System;
namespace Incapsulation.Weights
{
    /// <summary>
    /// Обертка для доступа
    /// к части массива
    /// с определенного индекса
    /// </summary>
    public class Indexer
    {
        private readonly double[] array;
        private readonly int start;
        private readonly int length;

        /// <summary>
        /// Конструктор обертки
        /// для массива
        /// </summary>
        /// <param name="array">
        /// Данный массив
        /// </param>
        /// <param name="start">
        /// Индекс начала
        /// </param>
        /// <param name="length">
        /// Длина отрезка
        /// </param>
        /// <exception cref="ArgumentException">
        /// Исключение при выходе за границы
        /// данного массива
        /// </exception>
        public Indexer(double[] array, int start, int length)
        {
            if (start < 0 || length < 0 || (start + length) >= array.Length)
            {
                throw new ArgumentException();
            }
            this.array = array;
            this.start = start;
            this.length = length;
        }

        /// <summary>
        /// Длина части массива
        /// в обертке
        /// </summary>
        public int Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Перезагрузка массивного
        /// доступа к обертке для
        /// изменения и получения данных
        /// из результирующего массива
        /// </summary>
        /// <param name="n">
        /// Индекс элемента
        /// в обертке
        /// </param>
        public double this[int n]
        {
            get
            {
                if (n < 0 || n >= Length)
                {
                    throw new IndexOutOfRangeException();
                }
                return array[start + n];
            }
            set
            {
                if (n < 0 || n >= Length)
                {
                    throw new IndexOutOfRangeException();
                }
                array[start + n] = value;
            }
        }
    }
}
