using System;
using System.Collections.Generic;

namespace Inheritance.DataStructure
{
    /// <summary>
    /// Категория сообщения
    /// </summary>
    /// <remarks>
    /// Содержит имя продукта, тип и 
    /// тему сообщения
    /// </remarks>
    public class Category : IComparable
    {
        public string ProductName;
        public MessageType Type;
        public MessageTopic Topic;

        /// <summary>
        /// Конструктор класса категории
        /// </summary>
        /// <param name="name">
        /// Имя продукта
        /// </param>
        /// <param name="type">
        /// Тип сообщения
        /// </param>
        /// <param name="topic">
        /// Тема сообщения
        /// </param>
        public Category(string name, MessageType type, MessageTopic topic)
        {
            ProductName = name;
            Type = type;
            Topic = topic;
        }

        /// <summary>
        /// Сравнивает категорию с 
        /// данным абстрактным объектом
        /// </summary>
        /// <param name="obj">
        /// Объект для сравнения
        /// </param>
        /// <returns>
        /// 1 - если текущая категория является
        /// приоритетнее данной для сравнения
        /// 0 - если объект для сравнения не
        /// является категорией или 
        /// если объекты равны
        /// -1 - если данная для сравнения категория
        /// является приорететнее текущей
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return 1;
            Category category = (Category)obj;
            if (this.Equals(obj))
                return 0;
            var productCompare = String.Compare(this.ProductName, category.ProductName);
            var typeCompare = this.Type.CompareTo(category.Type);
            var topicCompare = this.Topic.CompareTo(category.Topic);
            if (productCompare != 0) return productCompare;
            if (typeCompare != 0) return typeCompare;
            if (topicCompare != 0) return topicCompare;
            return 0;
        }

        /// <summary>
        /// Оператор "Меньше" для категорий
        /// </summary>
        /// <param name="left">
        /// Первая категория для сравнения
        /// </param>
        /// <param name="right">
        /// Вторая категория для сравнения
        /// </param>
        /// <returns>
        /// true - если первая категория
        /// не приоритетнее второй
        /// false - если первая категория
        /// приоритетнее второй
        /// </returns>
        public static bool operator <(Category left, Category right)
        {
            return left.CompareTo(right) == -1;
        }

        /// <summary>
        /// Оператор "Больше" для категорий
        /// </summary>
        /// <param name="left">
        /// Первая категория для сравнения
        /// </param>
        /// <param name="right">
        /// Вторая категория для сравнения
        /// </param>
        /// <returns>
        /// true - если первая категория
        /// приоритетнее второй
        /// false - если первая категория
        /// не приоритетнее второй
        /// </returns>
        public static bool operator >(Category left, Category right)
        {
            return left.CompareTo(right) == 1;
        }

        /// <summary>
        /// Оператор "Меньше или равно" для категорий
        /// </summary>
        /// <param name="left">
        /// Первая категория для сравнения
        /// </param>
        /// <param name="right">
        /// Вторая категория для сравнения
        /// </param>
        /// <returns>
        /// true - если вторая категория
        /// приоритетнее или 
        /// равноприоритетная с первой
        /// false - если первая категория
        /// приоритетнее второй
        /// </returns>
        public static bool operator <=(Category left, Category right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Оператор "Больше или равно" для категорий
        /// </summary>
        /// <param name="left">
        /// Первая категория для сравнения
        /// </param>
        /// <param name="right">
        /// Вторая категория для сравнения
        /// </param>
        /// <returns>
        /// true - если первая категория
        /// приоритетнее или 
        /// равноприоритетная со второй
        /// false - если вторая категория
        /// приоритетнее первой
        /// </returns>
        public static bool operator >=(Category left, Category right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Проверяет текущий объект
        /// на идентичность данному
        /// </summary>
        /// <param name="obj">
        /// Объект для сравнения
        /// </param>
        /// <returns>
        /// true - если объекты идентичны
        /// false - если объекты различны
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetHashCode() == this.GetHashCode();
        }

        /// <summary>
        /// Вычисляет хэш-код
        /// из полей категории
        /// </summary>
        /// <returns>
        /// Хэш-код
        /// из полей категории
        /// </returns>
        public override int GetHashCode()
        {
            if (ProductName == null) return base.GetHashCode();
            var hashCode = 10;
            hashCode = hashCode * 5 + ProductName.GetHashCode();
            hashCode = hashCode * 4 + Type.GetHashCode();
            hashCode = hashCode * 3 + Topic.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Метод для преобразования категории
        /// в строку
        /// </summary>
        /// <returns>
        /// Строковое представление
        /// категории
        /// </returns>
        public override string ToString()
        {
            return ProductName + '.' + Type + '.' + Topic;
        }
    }
}
