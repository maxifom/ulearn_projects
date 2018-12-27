using System;
using System.Numerics;

namespace Incapsulation.RationalNumbers
{
    /// <summary>
    /// Класс рационального числа
    /// </summary>
    /// <remarks>
    /// Содержит числитель
    /// и знаменатель
    /// </remarks>
    public class Rational
    {
        public int Numerator;
        public int Denominator;

        /// <summary>
        /// Конструктор рационального числа
        /// </summary>
        /// <param name="numerator">
        /// Числитель
        /// </param>
        /// <param name="denominator">
        /// Знаменатель
        /// </param>
        /// <remarks>
        /// Сокращает дробь если возможно
        /// и при отрицательном числе 
        /// переносит минус
        /// к числителю
        /// </remarks>
        public Rational(int numerator, int denominator = 1)
        {
            Numerator = numerator;
            Denominator = denominator;
            UpdateSign();
            ReduceFraction();
        }

        /// <summary>
        /// Перенос знака
        /// отрицательной дроби
        /// в числитель
        /// </summary>
        private void UpdateSign()
        {
            var sign = Numerator < 0 ^ Denominator < 0 ? -1 : 1;
            Numerator = sign * Math.Abs(Numerator);
            Denominator = Math.Abs(Denominator);
        }

        /// <summary>
        /// Сокращения дроби
        /// </summary>
        private void ReduceFraction()
        {
            int gcd = (int)BigInteger.GreatestCommonDivisor(Numerator, Denominator);
            if (gcd > 1)
            {
                Numerator /= gcd;
                Denominator /= gcd;
            }
        }

        /// <summary>
        /// Свойство для обозначения
        /// "нечисла"
        /// </summary>
        public bool IsNan
        {
            get
            {
                return Denominator == 0;
            }
        }

        /// <summary>
        /// Оператор сложения
        /// двух рациональных чисел
        /// </summary>
        /// <param name="x">
        /// Первое число
        /// </param>
        /// <param name="y">
        /// Второе число
        /// </param>
        /// <returns>
        /// Сумма двух чисел
        /// </returns>
        public static Rational operator +(Rational x, Rational y)
        {
            return new Rational(
                x.Numerator * y.Denominator + y.Numerator * x.Denominator,
                x.Denominator * y.Denominator
            );
        }

        /// <summary>
        /// Оператор вычитания
        /// двух рациональных чисел
        /// </summary>
        /// <param name="x">
        /// Первое число
        /// </param>
        /// <param name="y">
        /// Второе число
        /// </param>
        /// <returns>
        /// Разность двух чисел
        /// </returns>
        public static Rational operator -(Rational x, Rational y)
        {
            return new Rational(
                x.Numerator * y.Denominator - y.Numerator * x.Denominator,
                x.Denominator * y.Denominator
            );
        }

        /// <summary>
        /// Оператор умножения
        /// двух рациональных чисел
        /// </summary>
        /// <param name="x">
        /// Первое число
        /// </param>
        /// <param name="y">
        /// Второе число
        /// </param>
        /// <returns>
        /// Произведение двух чисел
        /// </returns>
        public static Rational operator *(Rational x, Rational y)
        {
            return new Rational(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
        }

        /// <summary>
        /// Оператор деления
        /// двух рациональных чисел
        /// </summary>
        /// <param name="x">
        /// Первое число
        /// </param>
        /// <param name="y">
        /// Второе число
        /// </param>
        /// <returns>
        /// Частное двух чисел
        /// либо второе число
        /// если деление невозможно
        /// </returns>
        public static Rational operator /(Rational x, Rational y)
        {
            return y.Denominator != 0 ?
                new Rational(x.Numerator * y.Denominator, x.Denominator * y.Numerator)
                : new Rational(y.Numerator, y.Denominator);
        }

        /// <summary>
        /// Оператор преобразования 
        /// целого числа к рациональному
        /// </summary>
        /// <param name="x">
        /// Целое число для преобразования
        /// в рациональное
        /// </param>
        /// <returns>
        /// Рациональное число 
        /// со знаменателем 1
        /// и числителем x
        /// </returns>
        public static implicit operator Rational(int x)
        {
            return new Rational(x);
        }

        /// <summary>
        /// Оператор преобразования
        /// рационального числа
        /// к вещественному числу
        /// </summary>
        /// <param name="x">
        /// Рациональное число
        /// </param>
        /// <returns>
        /// Вещественное представление
        /// рационального числа
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Ошибка при преобразовании "нечисла"
        /// в вещественное число
        /// </exception>
        public static implicit operator double(Rational x)
        {
            return !x.IsNan ? (double)x.Numerator / (double)x.Denominator : throw new ArgumentException();
        }

        /// <summary>
        /// Оператор преобразования
        /// рационального числа
        /// к целому числу
        /// </summary>
        /// <param name="x">
        /// Рациональное число
        /// </param>
        /// <returns>
        /// Целое представление
        /// рационального числа
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Ошибка при преобразовании 
        /// рационального числа с 
        /// числителем, отличным от 1
        /// в целое число
        /// </exception>
        public static explicit operator int(Rational x)
        {
            return x.Denominator == 1 ? x.Numerator : throw new ArgumentException();
        }
    }
}
