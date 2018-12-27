using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    /// <summary>
    /// Обходитель дерева
    /// </summary>
    /// <typeparam name="TTree">
    /// Тип дерева
    /// </typeparam>
    /// <typeparam name="TVal">
    /// Тип данных дерева
    /// </typeparam>
    public class Traversal<TTree, TVal>
    {
        Func<TTree, IEnumerable<TVal>> treeNodeHandler;
        Func<TTree, IEnumerable<TTree>> treeParser;
        Func<TTree, bool> isValid;

        /// <summary>
        /// Конструктор обходителя
        /// дерева
        /// </summary>
        /// <param name="treeNodeHandler">
        /// Функция для получения
        /// значения элемента дерева
        /// </param>
        /// <param name="treeParser">
        /// Функция для получения следующего
        /// элемента дерева
        /// </param>
        /// <param name="isValid">
        /// Функция для проверки на
        /// соответствие условию
        /// задачи
        /// </param>
        public Traversal(Func<TTree, IEnumerable<TVal>> treeNodeHandler,
                    Func<TTree, IEnumerable<TTree>> treeParser,
                    Func<TTree, bool> isValid)
        {
            this.treeNodeHandler = treeNodeHandler;
            this.treeParser = treeParser;
            this.isValid = isValid;
        }

        /// <summary>
        /// Обойти дерево
        /// </summary>
        /// <param name="tree">
        /// Дерево
        /// </param>
        /// <param name="resultList">
        /// Результирующий список
        /// </param>
        public void Traverse(TTree tree, List<TVal> resultList)
        {
            if (isValid(tree))
                resultList.AddRange(treeNodeHandler(tree));

            var next = treeParser(tree);
            foreach (var node in next)
                Traverse(node, resultList);
        }
    }

    /// <summary>
    /// Вызывает различные
    /// методы обхода деревьев
    /// </summary>
    public static class Traversal
    {
        /// <summary>
        /// Получает список задач
        /// без подзадач
        /// </summary>
        /// <param name="jobList">
        /// Список задач
        /// </param>
        /// <returns>
        /// Список задач без подзадач
        /// </returns>
        public static IEnumerable<Job> GetEndJobs(Job jobList)
        {
            Traversal<Job, Job> traversal = new Traversal<Job, Job>(
                job => new List<Job> { job },
                job => job.Subjobs,
                job => job.Subjobs == null || job.Subjobs.Count == 0
            );
            List<Job> endJobs = new List<Job>();
            traversal.Traverse(jobList, endJobs);
            return endJobs;
        }

        /// <summary>
        /// Получает все величины
        /// бинарного дерева
        /// </summary>
        /// <typeparam name="T">
        /// Тип данных бинарного
        /// дерева
        /// </typeparam>
        /// <param name="binaryTree">
        /// Бинарное дерево
        /// </param>
        /// <returns>
        /// Величины бинарного дерева
        /// </returns>
        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> binaryTree)
        {
            Traversal<BinaryTree<T>, T> traversal = new Traversal<BinaryTree<T>, T>(
                tree => new List<T> { tree.Value },
                tree =>
                {
                    var nodes = new List<BinaryTree<T>>();
                    if (tree.Left != null) nodes.Add(tree.Left);
                    if (tree.Right != null) nodes.Add(tree.Right);
                    return nodes;
                },
                tree => true);
            List<T> treeValues = new List<T>();
            traversal.Traverse(binaryTree, treeValues);
            return treeValues;
        }

        /// <summary>
        /// Получает список продуктов
        /// </summary>
        /// <param name="productData">
        /// Данные о продуктах
        /// </param>
        /// <returns>
        /// Список продуктов
        /// </returns>
        public static IEnumerable<Product> GetProducts(ProductCategory productData)
        {
            Traversal<ProductCategory, Product> traversal = new Traversal<ProductCategory, Product>(
                 productCategory => productCategory.Products,
                 productCategory => productCategory.Categories,
                 productCategory => true);
            List<Product> productList = new List<Product>();
            traversal.Traverse(productData, productList);
            return productList;
        }
    }
}
