using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    /// <summary>
    /// Бинарное дерево
    /// </summary>
    /// <typeparam name="T">
    /// Тип элемента в бинарном дереве
    /// </typeparam>
    /// <remarks>
    /// Элемент должен быть
    /// сравнимым
    /// </remarks>
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        public BinaryTree<T> Left;
        public BinaryTree<T> Right;
        public SortedSet<T> Tree;
        public T Value;
        
        /// <summary>
        /// Конструктор бинарного
        /// дерева
        /// </summary>
        /// <remarks>
        /// Создает пустой 
        /// список элементов
        /// и делает корневое 
        /// значение по умолчанию
        /// для данного типа
        /// </remarks>
        public BinaryTree()
        {
            this.Tree = new SortedSet<T>();
            this.Value = default(T);
        }

        /// <summary>
        /// Добавляет элемент
        /// в бинарное дерево
        /// </summary>
        /// <param name="element">
        /// Элемент для добавления
        /// </param>
        public void Add(T element)
        {
            if (this.Tree.Count == 0)
            {
                this.Value = element;
            }
            else if(this.Value.CompareTo(element) >= 0 )
            {
                if (this.Left == null) this.Left = new BinaryTree<T>();
                this.Left.Add(element);
            }
            else
            {
                if (this.Right == null) this.Right = new BinaryTree<T>();
                this.Right.Add(element);
            }
            this.Tree.Add(element);
        }

        /// <summary>
        /// Получает массивное представление
        /// бинарного дерева
        /// </summary>
        /// <returns>
        /// Массивное представление
        /// бинарного дерева
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Tree.GetEnumerator();
        }

        /// <summary>
        /// Получает массивное представление
        /// бинарного дерева
        /// </summary>
        /// <returns>
        /// Массивное представление
        /// бинарного дерева
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Tree.GetEnumerator();
        }
    }

    /// <summary>
    /// Бинарное дерево
    /// </summary>
    /// <remarks>
    /// Определяет тип элементов
    /// и создает бинарное дерево
    /// заданного типа
    /// </remarks>
    public class BinaryTree
    {
        /// <summary>
        /// Создание бинарного дерева
        /// для данных элементов
        /// </summary>
        /// <typeparam name="T">
        /// Тип элементов бинарного дерева
        /// </typeparam>
        /// <param name="numbers">
        /// Элементы для вставки в бинарное дерево
        /// </param>
        /// <returns>
        /// Новое бинарное дерево
        /// данного типа и с данными
        /// элементами
        /// </returns>
        public static BinaryTree<T> Create<T>(params T[] numbers) where T : IComparable
        {
            var binaryTree = new BinaryTree<T>();
            for (var i=0;i<numbers.Length;i++)
            {
                binaryTree.Add(numbers[i]);
            }
            return binaryTree;
        }
    }
}
