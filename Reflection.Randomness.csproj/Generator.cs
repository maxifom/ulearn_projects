using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Reflection.Randomness
{
    /// <summary>
    /// Свойство распределения
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FromDistribution : Attribute
    {
        private Type type;
        private object[] args;

        /// <summary>
        /// Контсруктор свойства
        /// распределения
        /// </summary>
        /// <param name="distributionType">
        /// Тип распределения
        /// </param>
        /// <param name="args">
        /// Аргументы распределения
        /// </param>
        public FromDistribution(Type distributionType, params object[] args)
        {
            this.type = distributionType;
            this.args = args;
        }

        /// <summary>
        /// Создает новое
        /// распределение
        /// </summary>
        /// <returns>
        /// Распределение
        /// </returns>
        public IContinousDistribution Create()
        {
            try
            {
                return (IContinousDistribution)Activator.CreateInstance(type, args);
            }
            catch
            {
                throw new ArgumentException(type.FullName);
            }
        }
    }

    /// <summary>
    /// Генератор
    /// случайных чисел
    /// </summary>
    /// <typeparam name="T">
    /// Тип данных для генератора
    /// </typeparam>
    public class Generator<T>
    {
        public static Dictionary<PropertyInfo, Lazy<IContinousDistribution>> LazyDictionary;
        public Dictionary<PropertyInfo, IContinousDistribution> Dictionary;

        /// <summary>
        /// Статический конструктор
        /// генератора случайных чисел
        /// </summary>
        static Generator()
        {
            LazyDictionary = new Dictionary<PropertyInfo, Lazy<IContinousDistribution>>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (Attribute.GetCustomAttribute(property, typeof(FromDistribution)) is FromDistribution attribute)
                {
                    Lazy<IContinousDistribution> lazyDistribution = new Lazy<IContinousDistribution>(() => attribute.Create());
                    LazyDictionary.Add(property, lazyDistribution);
                }
                else
                {
                    LazyDictionary.Add(property, null);
                }
            }
        }

        /// <summary>
        /// Конструктор 
        /// генератора случайных
        /// чисел
        /// </summary>
        public Generator()
        {
            this.Dictionary = new Dictionary<PropertyInfo, IContinousDistribution>();
        }

        /// <summary>
        /// Генерирует новое
        /// случайное число
        /// </summary>
        /// <param name="random">
        /// Объект для генерации
        /// случайных чисел
        /// </param>
        /// <returns>
        /// Новое случайное число 
        /// </returns>
        public T Generate(Random random)
        {
            var generator = Activator.CreateInstance(typeof(T));
            var distibutionValue = 0.0;

            foreach (var key in LazyDictionary.Keys)
            {
                if (Dictionary.ContainsKey(key))
                {
                    distibutionValue = Dictionary[key].Generate(random);
                    key.SetValue(generator, distibutionValue);
                }
                else if (LazyDictionary[key] != null)
                {
                    distibutionValue = LazyDictionary[key].Value.Generate(random);
                    key.SetValue(generator, distibutionValue);
                }
            }

            return (T)generator;
        }

        /// <summary>
        /// Возвращает новый
        /// временный объект 
        /// из выражения
        /// </summary>
        /// <param name="expression">
        /// Выражение
        /// </param>
        /// <returns>
        /// Новый временный объект
        /// </returns>
        public Temp<T> For(Expression<Func<T, object>> expression)
        {
            try
            {
                Type type = expression.GetType();
                Expression expressionBody = expression.Body;
                UnaryExpression unaryExpression = (UnaryExpression)expressionBody;
                MemberExpression memberExpression = (MemberExpression)unaryExpression.Operand;
                var name = memberExpression.Member.Name;
                PropertyInfo prop = typeof(T).GetProperty(name);
                if (prop == null)
                    throw new ArgumentException();
                return new Temp<T>(prop, this);
            }
            catch
            {
                throw new ArgumentException();
            }
        }
    }

    /// <summary>
    /// Временный объект
    /// </summary>
    /// <typeparam name="T">
    /// Тип данных для генератора
    /// </typeparam>
    public class Temp<T>
    {
        private readonly PropertyInfo property;
        private readonly Generator<T> generator;

        /// <summary>
        /// Конструктор временного
        /// объекта
        /// </summary>
        /// <param name="property">
        /// Свойство
        /// </param>
        /// <param name="generator">
        /// Генератор случаных чисел
        /// </param>
        public Temp(PropertyInfo property, Generator<T> generator)
        {
            this.property = property;
            this.generator = generator;
        }

        /// <summary>
        /// Добавляет свойство
        /// объекта в словарь
        /// генератора
        /// </summary>
        /// <param name="distribution">
        /// Распределение
        /// </param>
        /// <returns>
        /// Генератор случайных чисел
        /// данного временного объекта
        /// </returns>
        public Generator<T> Set(IContinousDistribution distribution)
        {
            generator.Dictionary.Add(this.property, distribution);
            return generator;
        }
    }
}