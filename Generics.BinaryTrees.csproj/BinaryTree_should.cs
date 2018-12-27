using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
	[TestFixture]
	public class BinaryTree_should
	{
		[Test]
		public void BeEmptyWhenCreated()
		{
			CollectionAssert.AreEqual(new int[0], new BinaryTree<int>());
		}

        [Test]
        public void PlaceLesserElementToLeft()
        {
            var tree = new BinaryTree<int>();
            tree.Add(2);
            tree.Add(1);
            Assert.AreEqual(2, tree.Value);
            Assert.AreEqual(1, tree.Left.Value);
        }

        [Test]
        public void PlaceEqualElementToLeft()
        {
            var tree = new BinaryTree<int>();
            tree.Add(2);
            tree.Add(2);
            Assert.AreEqual(2, tree.Value);
            Assert.AreEqual(2, tree.Left.Value);
        }

        [Test]
        public void PlaceGreaterElementToRight()
        {
            var tree = new BinaryTree<int>();
            tree.Add(2);
            tree.Add(3);
            Assert.AreEqual(2, tree.Value);
            Assert.AreEqual(3, tree.Right.Value);
        }

        [Test]
        public void InitializeFromAnArrayAndSort1()
        {
            // Чтобы этот код скомпилировался, нужно создать ещё один 
            // класс с именем BinaryTree, но уже без generic-аргументов.
            // Так делают, чтобы можно было не указывать тип-параметр при использовании.
            // Дело в том, что типы-параметры методов часто компилятор может 
            // определить самостоятельно и их можно не указывать, 
            // а типы-параметры у самого типа или его конструктора, указывать нужно всегда.
            // Пример применения такой техники в стандартной библиотеки — 
            // это классы Tuple<T1, T2> и класс Tuple со статическим методоми Create<T1, T2>(...).
            var tree = BinaryTree.Create(4, 3, 2, 1);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, tree);
        }

        [Test]
        public void InitializeFromAnArrayAndSort2()
        {
            var tree = BinaryTree.Create(2, 4, 1, 7, 3, 9, 5, 6, 8);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, tree);
        }

        [Test]
        public void CanStoreDefaultValueOfT()
        {
            var tree = BinaryTree.Create(1, 0, 2);
            CollectionAssert.AreEqual(new[] { 0, 1, 2 }, tree);
        }

        [Test]
        public void EnumerationIsLazy()
        {
            var r = new Random();
            var items = Enumerable.Range(0, 50000).Select(i => r.Next());
            var tree = BinaryTree.Create(items.ToArray());
            RunWithTimeout(1000, "tree.First() is too slow. Enumeration is not lazy?",
                () =>
                {
                    var sum = 0;
                    for (int i = 0; i < 50000; i++)
                    {
                        sum += tree.First();
                    }
                });
        }

        protected static void RunWithTimeout(int timeout, string message, Action func)
        {
            var task = Task.Run(func);
            if (!task.Wait(timeout))
            {
                Assert.Fail(message);
            }
        }
    }
}
