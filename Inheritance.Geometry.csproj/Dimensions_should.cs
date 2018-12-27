using NUnit.Framework;
using System;

namespace Inheritance.Geometry
{
	[TestFixture]
	public class Dimensions_should
	{
		[Test]
		public void BeCorrectForBall()
		{
			var body = new Ball { Radius = 2 };
			var dimensions = body.GetDimensions();
			Assert.AreEqual(4, dimensions.Width, 1e-7);
			Assert.AreEqual(4, dimensions.Height, 1e-7);
		}

		[Test]
		public void BeCorrectForCube()
		{
			var body = new Cube { Size = 3 };
			var dimensions = body.GetDimensions();
			Assert.AreEqual(3, dimensions.Width, 1e-7);
			Assert.AreEqual(3, dimensions.Height, 1e-7);
		}

		[Test]
		public void BeCorrectForCyllinder()
		{
			var body = new Cyllinder { Radius = 2, Height = 3 };
			var dimensions = body.GetDimensions();
			Assert.AreEqual(4, dimensions.Width, 1e-7);
			Assert.AreEqual(3, dimensions.Height, 1e-7);
		}
	}
}