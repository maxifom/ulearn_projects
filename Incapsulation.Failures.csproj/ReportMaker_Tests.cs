using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    [TestFixture]
    public class ReportMaker_Tests
    {
        [Test]
        public void NoDevices()
        {
            var result = ReportMaker.FindDevicesFailedBeforeDateObsolete(
                10, 5, 2010,
                new int[] { },
                new int[] { },
                new object[][] { },
                new List<Dictionary<string, object>>());
            CollectionAssert.IsEmpty(result);
        }

        [Test]
        public void FilterSingleDevice()
        {
            var result = ReportMaker.FindDevicesFailedBeforeDateObsolete(
                 10,
                 5,
                 2010,
                 new[] { 0, 1, 2, 3 },
                 new[] { 0, 1, 2, 3 },
                 new[]
                 {
                    new object[] { 9,4,2010 },
                    new object[] { 9,4,2010 },
                    new object[] { 11, 5, 2010 },
                    new object[] {9,4,2010 }
                 },
                 new List<Dictionary<string, object>>
                 {
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=0,
                        ["Name"]="0"
                    },
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=1,
                        ["Name"]="1"
                    },
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=2,
                        ["Name"]="2"
                    },
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=3,
                        ["Name"]="3"
                    },
                 });

            CollectionAssert.AreEqual(new[] { "0" }, result);
        }

        [Test]
        public void FilterManyDevices()
        {
            var result = ReportMaker.FindDevicesFailedBeforeDateObsolete(
                10,
                5,
                2010,
                new[] { 0, 1, 0, 2 },
                new[] { 0, 1, 2, 3 },
                new[]
                {
                    new object[] { 9,5,2010 },
                    new object[] { 1,6,2009 },
                    new object[] { 9, 5, 2010 },
                    new object[] { 19,5,2010 }
                },
                new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=0,
                        ["Name"]="Device A"
                    },
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=1,
                        ["Name"]="Device B"
                    },
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=2,
                        ["Name"]="Device C"
                    },
                    new Dictionary<string, object>
                    {
                        ["DeviceId"]=3,
                        ["Name"]="Device D"
                    },
                });

            CollectionAssert.AreEqual(new[] { "Device A", "Device C" }, result);
        }
    }
}
