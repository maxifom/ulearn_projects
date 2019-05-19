using NUnit.Framework;

namespace SRP.ControlDigit
{
	[TestFixture]
	public class ControlDigitAlgo_Tests
	{
		[Test]
		public void TestUpc()
		{
			Assert.AreEqual(0, ControlDigitAlgo.Upc(0));
			Assert.AreEqual(7, ControlDigitAlgo.Upc(1));
			Assert.AreEqual(4, ControlDigitAlgo.Upc(2));
			Assert.AreEqual(1, ControlDigitAlgo.Upc(3));
			Assert.AreEqual(8, ControlDigitAlgo.Upc(4));
			Assert.AreEqual(5, ControlDigitAlgo.Upc(5));
			Assert.AreEqual(2, ControlDigitAlgo.Upc(6));
			Assert.AreEqual(9, ControlDigitAlgo.Upc(7));
			Assert.AreEqual(6, ControlDigitAlgo.Upc(8));
			Assert.AreEqual(3, ControlDigitAlgo.Upc(9));
			Assert.AreEqual(9, ControlDigitAlgo.Upc(10));
			Assert.AreEqual(6, ControlDigitAlgo.Upc(11));
			Assert.AreEqual(3, ControlDigitAlgo.Upc(12));
			Assert.AreEqual(0, ControlDigitAlgo.Upc(13));
			Assert.AreEqual(7, ControlDigitAlgo.Upc(14));
			Assert.AreEqual(4, ControlDigitAlgo.Upc(15));
			Assert.AreEqual(1, ControlDigitAlgo.Upc(16));
			Assert.AreEqual(8, ControlDigitAlgo.Upc(17));
			Assert.AreEqual(5, ControlDigitAlgo.Upc(18));
			Assert.AreEqual(2, ControlDigitAlgo.Upc(19));
			Assert.AreEqual(7, ControlDigitAlgo.Upc(03600024145));
			Assert.AreEqual(5, ControlDigitAlgo.Upc(01010101010));
		}

        [Test]
        public void TestIsbn10()
        {
            Assert.AreEqual('0', ControlDigitAlgo.Isbn10(0));
            Assert.AreEqual('1', ControlDigitAlgo.Isbn10(020153082));
            Assert.AreEqual('9', ControlDigitAlgo.Isbn10(1));
            Assert.AreEqual('7', ControlDigitAlgo.Isbn10(2));
            Assert.AreEqual('5', ControlDigitAlgo.Isbn10(3));
            Assert.AreEqual('1', ControlDigitAlgo.Isbn10(5));
            Assert.AreEqual('X', ControlDigitAlgo.Isbn10(6));
            Assert.AreEqual('6', ControlDigitAlgo.Isbn10(11));
        }
        [Test]
        public void TestIsbn13()
        {
            Assert.AreEqual(0, ControlDigitAlgo.Isbn13(0));
            Assert.AreEqual(9, ControlDigitAlgo.Isbn13(1));
            Assert.AreEqual(8, ControlDigitAlgo.Isbn13(2));
            Assert.AreEqual(7, ControlDigitAlgo.Isbn13(3));
            Assert.AreEqual(6, ControlDigitAlgo.Isbn13(4));
            Assert.AreEqual(5, ControlDigitAlgo.Isbn13(5));
            Assert.AreEqual(4, ControlDigitAlgo.Isbn13(6));
            Assert.AreEqual(3, ControlDigitAlgo.Isbn13(7));
            Assert.AreEqual(2, ControlDigitAlgo.Isbn13(8));
            Assert.AreEqual(1, ControlDigitAlgo.Isbn13(9));
            Assert.AreEqual(7, ControlDigitAlgo.Isbn13(10));
            Assert.AreEqual(6, ControlDigitAlgo.Isbn13(11));
            Assert.AreEqual(5, ControlDigitAlgo.Isbn13(12));
            Assert.AreEqual(4, ControlDigitAlgo.Isbn13(13));
            Assert.AreEqual(3, ControlDigitAlgo.Isbn13(14));
            Assert.AreEqual(2, ControlDigitAlgo.Isbn13(15));
            Assert.AreEqual(1, ControlDigitAlgo.Isbn13(16));
            Assert.AreEqual(0, ControlDigitAlgo.Isbn13(17));
            Assert.AreEqual(9, ControlDigitAlgo.Isbn13(18));
            Assert.AreEqual(8, ControlDigitAlgo.Isbn13(19));
            Assert.AreEqual(3, ControlDigitAlgo.Isbn13(03600024145));
            Assert.AreEqual(5, ControlDigitAlgo.Isbn13(01010101010));
        }
    }
}