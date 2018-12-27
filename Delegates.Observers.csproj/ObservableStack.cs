using System;
using System.Collections.Generic;
using System.Text;

namespace Delegates.Observers
{
    /// <summary>
    /// Логгер операций
    /// со стеком
    /// </summary>
    public class StackOperationsLogger
    {
        /// <summary>
        /// Логгер для записи
        /// операций
        /// </summary>
        public StringBuilder Logger = new StringBuilder();

        /// <summary>
        /// Получить строковое представление
        /// логгера
        /// </summary>
        /// <returns>
        /// Строковое представление логгера
        /// </returns>
        public string GetLog() => Logger.ToString();

        /// <summary>
        /// Подписаться на события
        /// стека
        /// </summary>
        /// <typeparam name="T">
        /// Тип данных в стеке
        /// </typeparam>
        /// <param name="stack">
        /// Стек
        /// </param>
        public void SubscribeOn<T>(ObservableStack<T> stack)
        {
            stack.OnStackChanged += (sender, e) =>
            {
                Logger.Append(e);
            };
        }
    }

    /// <summary>
    /// Стек с возможностью
    /// наблюдения
    /// </summary>
    /// <typeparam name="T">
    /// Тип данных в стеке
    /// </typeparam>
    public class ObservableStack<T>
    {
        /// <summary>
        /// Событие изменения
        /// стека
        /// </summary>
        public event EventHandler<StackEventData<T>> OnStackChanged;

        /// <summary>
        /// Данные стека
        /// </summary>
        List<T> stackData = new List<T>();

        /// <summary>
        /// Вызывает событие
        /// изменение стека
        /// </summary>
        /// <param name="eventData">
        /// Данные события
        /// </param>
        protected void Notify(StackEventData<T> eventData)
        {
            OnStackChanged?.Invoke(this, eventData);
        }

        /// <summary>
        /// Удаляет последний элемент
        /// стека
        /// </summary>
        /// <returns>
        /// Удаленный элемент стека
        /// </returns>
        public T Pop()
        {
            if (stackData.Count == 0)
                throw new InvalidOperationException();
            var last_element = stackData[stackData.Count - 1];
            Notify(new StackEventData<T> { IsPushed = false, Value = last_element });
            return last_element;
        }

        /// <summary>
        /// Добавляет элемент
        /// в конец стека
        /// </summary>
        /// <param name="element">
        /// Элемент для добавления
        /// </param>
        public void Push(T element)
        {
            stackData.Add(element);
            Notify(new StackEventData<T> { IsPushed = true, Value = element });
        }
    }
}