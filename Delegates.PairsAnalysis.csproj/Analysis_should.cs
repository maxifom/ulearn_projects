using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    [TestFixture]
    public class Analysis_should
    {
        [Test]
        public void ProcessDatesCorrectly()
        {
            Assert.AreEqual(
                2,
                Analysis.FindMaxPeriodIndex(
                    new DateTime(2001, 1, 11),
                    new DateTime(2001, 1, 12),
                    new DateTime(2001, 1, 13),
                    new DateTime(2001, 1, 20),
                    new DateTime(2001, 1, 21)));
        }

        [Test]
        public void ThrowsAtEmptyCollection()
        {
            Assert.Throws(typeof(ArgumentException),() => Analysis.FindMaxPeriodIndex());
        }

        [Test]
        public void ThrowsAtOneElementCollection()
        {
            Assert.Throws(typeof(ArgumentException), () => Analysis.FindMaxPeriodIndex(new DateTime(2001, 1, 1)));
        }

        [Test]
        public void ProcessDoublesCorrectly()
        {
            Assert.AreEqual(
                0.1,
                Analysis.FindAverageRelativeDifference(
                    1,
                    1.1,
                    1.21,
                    1.331),
                1e-5);
        }

    }
}
