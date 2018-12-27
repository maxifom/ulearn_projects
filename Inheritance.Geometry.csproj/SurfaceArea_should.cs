using NUnit.Framework;
using System;

namespace Inheritance.Geometry
{
	[TestFixture]
	public class SurfaceArea_should
	{
		[Test]
		public void BeCorrectForBall()
		{
			var body = new Ball { Radius = 2 };
			Assert.AreEqual(16 * Math.PI, body.GetSurfaceArea(), 1e-7);
		}

		[Test]
		public void BeCorrectForCube()
		{
			var body = new Cube { Size = 3 };
			Assert.AreEqual(54, body.GetSurfaceArea(), 1e-7);
		}

		[Test]
		public void BeCorrectForCyllinder()
		{
			var body = new Cyllinder {Radius = 2, Height = 3};
			Assert.AreEqual(20 * Math.PI, body.GetSurfaceArea(), 1e-7);
		}
	}
}