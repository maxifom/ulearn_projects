using Ddd.Taxi.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ddd.Infrastructure
{
    /// <summary>
    /// Базовый класс для всех Value типов.
    /// </summary>
    public class ValueType<T>
    {
        private readonly List<PropertyInfo> orderedProperties;

        /// <summary>
        /// Конструктор Value Type
        /// Получает и сортирует все свойства в список
        /// </summary>
        public ValueType()
        {
            this.orderedProperties = this.GetType()
                                         .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                         .OrderBy(p => p.Name)
                                         .ToList();
        }

        /// <summary>
        /// Сравнивает текущий объект с данным
        /// </summary>
        /// <param name="obj">
        /// Объект для сравнения
        /// </param>
        /// <returns>
        /// Результат сравнения текущего объекта с данным
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            foreach (var property in orderedProperties)
            {
                var thisValue = property.GetValue(this, null);
                var objectValue = property.GetValue(obj, null);
                if (thisValue == null & objectValue == null)
                    continue;
                if (thisValue == null || objectValue == null || !thisValue.Equals(objectValue))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Вычисляет хеш значение объекта
        /// по его свойствам
        /// </summary>
        /// <returns>
        /// Хеш значение объекта
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 0;
            unchecked
            {
                foreach (var property in orderedProperties)
                {
                    var propertyHash = property.GetValue(this, null).GetHashCode();
                    hash = (hash * 1248188) ^ propertyHash;
                }
            }

            return hash;
        }

        public bool Equals(PersonName name) => Equals((object)name);

        /// <summary>
        /// Получает строковое представление
        /// по названию класса и свойствам объекта
        /// </summary>
        /// <returns>
        /// Строковое представление объекта
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder(this.GetType().Name + "(");
            int index = 0;
            foreach (var property in orderedProperties)
            {
                if (index != orderedProperties.Count - 1)
                    result.AppendFormat("{0}: {1}; ", property.Name, property.GetValue(this, null));
                else
                    result.AppendFormat("{0}: {1})", property.Name, property.GetValue(this, null));
                index++;
            }
            return result.ToString();
        }
    }
}