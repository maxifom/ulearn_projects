using NUnit.Framework;
using System;

namespace Inheritance.Geometry
{
    [TestFixture]
    public class Volume_should
    {
        [Test]
        public void BeCorrectForBall()
        {
            Assert.AreEqual(4 * Math.PI * 8 / 3, new Ball { Radius = 2 }.GetVolume(), 1e-7);
        }

        [Test]
        public void BeCorrectForCube()
        {
            Assert.AreEqual(27, new Cube { Size = 3 }.GetVolume(), 1e-7);
        }

        [Test]
        public void BeCorrectForCyllinder()
        {
            Assert.AreEqual(Math.PI*4*3, new Cyllinder { Radius=2, Height=3 }.GetVolume(), 1e-7);
        }

    }
}
