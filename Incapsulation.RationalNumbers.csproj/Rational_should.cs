using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    [TestFixture]
    public class Rational_should
    {
        public void Equal(int numerator, int denominator, Rational b)
        {
            Assert.False(b.IsNan);
            Assert.AreEqual(numerator, b.Numerator);
            Assert.AreEqual(denominator, b.Denominator);
        }

        [Test]
        public void InitializeSimpleRatioCorrectly()
        {
            Equal(1, 2, new Rational(1, 2));
        }

        [Test]
        public void InitializeWitoutDenomerator()
        {
            Equal(4, 1, new Rational(4));
        }

        [Test]
        public void InitializeWithZeroDenomerator()
        {
            Assert.True(new Rational(2, 0).IsNan);
        }

        public void BeCorrectWithZeroNumerator()
        {
            Equal(0, 1, new Rational(0, 5));
        }

        [Test]
        public void InitializeAndReduce1()
        {
            Equal(1, 2, new Rational(2, 4));
        }

        [Test]
        public void InitializeAndReduce2()
        {
            Equal(-1, 2, new Rational(-2, 4));
        }

        [Test]
        public void InitializeAndReduce3()
        {
            Equal(-1, 2, new Rational(2, -4));
        }

        [Test]
        public void InitializeAndReduce4()
        {
            Equal(1, 2, new Rational(-2, -4));
        }

        [Test]
        public void Sum()
        {
            Equal(1, 2, new Rational(1, 4) + new Rational(1, 4));
        }

        [Test]
        public void SumWithNan()
        {
            Assert.True((new Rational(1, 2) + new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) + new Rational(1, 2)).IsNan);
        }

        [Test]
        public void Subtract()
        {
            Equal(1, 4, new Rational(1, 2) - new Rational(1, 4));
        }

        [Test]
        public void SubtractWithNan()
        {
            Assert.True((new Rational(1, 2) - new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) - new Rational(1, 2)).IsNan);
        }


        [Test]
        public void Multiply()
        {
            Equal(-1, 4, new Rational(-1, 2) * new Rational(1, 2));
        }

        [Test]
        public void MultiplyWithNan()
        {
            Assert.True((new Rational(1, 2) * new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0) * new Rational(1, 2)).IsNan);
        }


        [Test]
        public void Divide()
        {
            Equal(-1, 2, new Rational(1,4) / new Rational(-1,2));
        }

        [Test]
        public void DivideWithNan()
        {
            Assert.True((new Rational(1, 2) / new Rational(1, 0)).IsNan);
            Assert.True((new Rational(1, 0)/ new Rational(1, 2)).IsNan);
        }

        [Test]
        public void DivideToZero()
        {
            Assert.True((new Rational(1, 2) / new Rational(0, 5)).IsNan);
        }

        [Test]
        public void ConvertToDouble()
        {
            double v = new Rational(1, 2);
            Assert.AreEqual(0.5, v, 1e-7);
        }

        [Test]
        public void ConvertFromInt()
        {
            Rational r = 5;
            Equal(5, 1, r);
        }

        [Test]
        public void ExplicitlyConvertToInt()
        {
            int a = (int)new Rational(2, 1);
            Assert.AreEqual(2, a);
        }

        [Test]
        public void ExplicitlyConvertToIntAndFailsIfNonCorvertable()
        {
            Assert.Throws(typeof(ArgumentException), () => { int a = (int)new Rational(1, 2); });
        }
    }
}
