using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    [TestFixture]
    public class Indexer_should
    {
        private readonly double[] array = { 1, 2, 3, 4 };

        [Test]
        public void HaveCorrectLength()
        {
            // 1, [ 2, 3, ] 4
            var indexer = new Indexer(array, start:1, length:2);
            Assert.AreEqual(2, indexer.Length);
        }

        [Test]
        public void GetCorrectly()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.AreEqual(2, indexer[0]);
            Assert.AreEqual(3, indexer[1]);
        }

        [Test]
        public void SetCorrectly()
        {
            var indexer = new Indexer(array, 1, 2);
            indexer[0] = 10;
            Assert.AreEqual(10, array[1]);
        }

        [Test]
        public void IndexerDoesNotCopyArray()
        {
            // 1, [ 2, 3,] 4
            var indexer1 = new Indexer(array, 1, 2);
            // [1,  2,] 3,  4
            var indexer2 = new Indexer(array, 0, 2);
            indexer1[0] = 100500;
            Assert.AreEqual(100500, indexer2[1]);
        }

        [Test]
        public void ConstructorFails_WhenRangeIsInvalid1()
        {
            Assert.Throws(typeof(ArgumentException), () => new Indexer(array, -1, 3));
        }

        [Test]
        public void ConstructorFails_WhenRangeIsInvalid2()
        {
            Assert.Throws(typeof(ArgumentException), () => new Indexer(array, 1, -1));
        }

        [Test]
        public void ConstructorFails_WhenRangeIsInvalid3()
        {
            Assert.Throws(typeof(ArgumentException), () => new Indexer(array, 1, 10));
        }

        [Test]
        public void IndexerGetter_Fails_WhenIndexIsTooSmall()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { var a = indexer[-1]; });
        }
        [Test]
        public void IndexerGetter_Fails_WhenIndexIsTooBig()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { var a = indexer[2]; });
        }

        [Test]
        public void IndexerSetter_Fails_WhenIndexIsTooSmall()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { indexer[-1] = 1; });
        }

        [Test]
        public void IndexerSetter_Fails_WhenIndexIsTooBig()
        {
            var indexer = new Indexer(array, 1, 2);
            Assert.Throws(typeof(IndexOutOfRangeException), () => { indexer[2] = 1; });
        }
    }
}
