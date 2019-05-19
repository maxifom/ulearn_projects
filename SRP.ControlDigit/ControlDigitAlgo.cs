namespace SRP.ControlDigit
{
    public static class ControlDigitAlgo
    {
        /// <summary>
        /// Считает контрольную цифру Универсального Кода Товара (UPC) 
        /// </summary>
        /// <param name="number">
        /// Универсальный Код Товара без контрольной цифры
        /// </param>
        /// <returns>
        /// Контрольную цифру Универсального Кода Товара
        /// </returns>
        public static int Upc(long number)
        {
            var result = number.SumOfOddPositions() * 3 + number.SumOfEvenPositions();
            var reminder = result % 10;
            return reminder == 0 ? 0 : 10 - reminder;
        }

        /// <summary>
        /// Считает контрольную цифру ISBN 10
        /// </summary>
        /// <param name="number">
        /// Код ISBN 10 без контрольной цифры
        /// </param>
        /// <returns>
        /// Контрольную цифру ISBN 10
        /// </returns>
        public static char Isbn10(long number)
        {
            var result = (11 - (number.SumOfDigitByDescPositionMultiplications() % 11)) % 11;
            return result == 10 ? 'X' : result.ToString()[0];
        }

        /// <summary>
        /// Считает контрольную цифру ISBN 13
        /// </summary>
        /// <param name="number">
        /// Код ISBN 10 без контрольной цифры
        /// </param>
        /// <returns>
        /// Контрольную цифру ISBN 13
        /// </returns>
        public static int Isbn13(long number)
        {
            var result = number.SumOfOddPositions() + number.SumOfEvenPositions() * 3;
            var reminder = result % 10;
            return reminder == 0 ? 0 : 10 - reminder;
        }
    }

    /// <summary>
    /// Расширения для работы с длинным целым числом
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Считает сумму цифр на нечетных позициях
        /// </summary>
        /// <param name="number">
        /// Число для рассчетов
        /// </param>
        /// <returns>
        /// Сумму цифр на нечетных позициях
        /// </returns>
        public static int SumOfOddPositions(this long number)
        {
            int i = 1; int sum = 0;
            while (number > 0)
            {
                var digit = (int)(number % 10);
                sum += i % 2 == 1 ? digit : 0;
                number /= 10; i++;
            }
            return sum;
        }

        /// <summary>
        /// Считает сумму цифр на четных позициях
        /// </summary>
        /// <param name="number">
        /// Число для рассчетов
        /// </param>
        /// <returns>
        /// Сумму цифр на четных позициях
        /// </returns>
        public static int SumOfEvenPositions(this long number)
        {
            int i = 1; int sum = 0;
            while (number > 0)
            {
                var digit = (int)(number % 10);
                sum += i % 2 == 0 ? digit : 0;
                number /= 10; i++;
            }
            return sum;
        }

        /// <summary>
        /// Считает сумму цифр, умноженных на свою позицию в числе с конца
        /// </summary>
        /// <param name="number">
        /// Число для рассчетов
        /// </param>
        /// <returns>
        /// Сумму цифр, умноженных на свою позицию в числе с конца
        /// </returns>
        public static int SumOfDigitByDescPositionMultiplications(this long number)
        {
            int i = 2; int sum = 0;
            while (number > 0)
            {
                var digit = (int)(number % 10);
                sum += digit * i;
                number /= 10; i++;
            }
            return sum;
        }
    }
}
