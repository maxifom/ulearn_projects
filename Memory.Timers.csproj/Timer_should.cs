using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Memory.Timers
{
    [TestFixture]
    public class Timer_should
    {
        List<int> Validate(string expected)
        {
            var actual = Timer.Report;
            expected = expected.Replace("#", "(\\d+)");
            var regex = new Regex(expected);
            var match = regex.Match(actual);
            if (!match.Success)
                Assert.Fail($"Your string \n{actual} does not match pattern {expected}");
            var result = match.Groups.Cast<Group>().Skip(1).Select(z => int.Parse(z.Value)).ToList();
            return result;
        }

        [Test]
        public void WorkInSimplestCase()
        {
            using (Timer.Start())
            {
            }
            Validate(@"\*[ ]{19}: (\d+)\n");
            /*Пример ответа
            *                   : 0
            */
        }

        [Test]
        public void WorkWithTimerName()
        {
            using (Timer.Start("MyTimer"))
            { }
            Validate(@"MyTimer[ ]{13}: (\d+)\n");
            /*Пример ответа
            MyTimer             : 0
            */
        }

        [Test]
        public void WorkWithNesting()
        {
            using (Timer.Start("A"))
            {
                using (Timer.Start("B"))
                { }
                using (Timer.Start("C"))
                { }
            }
            Validate(@"A[ ]{19}: (\d+)\n[ ]{4}B[ ]{15}: (\d+)\n[ ]{4}C[ ]{15}: (\d+)\n[ ]{4}Rest[ ]{12}: (\d+)");
            /* Пример ответа
            A                   : 0
                B               : 0
                C               : 0
                Rest            : 0
            */
        }

        [Test]
        public void WorkWithDeepNesting()
        {
            using (Timer.Start("A"))
            {
                using (Timer.Start("B"))
                {
                    using (Timer.Start("C"))
                    { }
                }
            }
            Validate(@"A[ ]{19}: (\d+)\n[ ]{4}B[ ]{15}: (\d+)\n[ ]{8}C[ ]{11}: (\d+)\n[ ]{8}Rest[ ]{8}: (\d+)\n[ ]{4}Rest[ ]{12}: (\d+)\n");

            /* Пример ответа
            A                   : 0
                B               : 0
                    C           : 0
                    Rest        : 0
                Rest            : 0
            */
        }

        [Test]
        public void WorkWithNestingAndGiveCorrectTime()
        {
            using (Timer.Start("A"))
            {
                using (Timer.Start("B"))
                {
                    Thread.Sleep(100);
                }
                using (Timer.Start("C"))
                {
                    Thread.Sleep(200);
                }
                Thread.Sleep(300);
            }
            var values = Validate(@"A[ ]{19}: (\d+)\n[ ]{4}B[ ]{15}: (\d+)\n[ ]{4}C[ ]{15}: (\d+)\n[ ]{4}Rest[ ]{12}: (\d+)");
            Assert.True(values[0] == values[1] + values[2] + values[3]);
            Assert.AreEqual(2, (double)values[2] / values[1], 1);
            Assert.AreEqual(3, (double)values[3] / values[1], 1);
            /*Пример ответа
            A                   : 600
                B               : 100
                C               : 200
                Rest            : 300
            */
        }
    }
}
