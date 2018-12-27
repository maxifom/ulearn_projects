using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Compression
{
    public class CustomCompressionStream_should
    {
        void Test()
        {
            var baseStream = new MemoryStream();
            var writer = new CustomCompressionStream(baseStream, false);
            var data = new List<byte>();
            var random = new Random();
            for (int i = 0; i < 50 + random.Next(100); i++)
            {
                var bt = (byte)random.Next(255);
                for (int j = 0; j < 2 + random.Next(2); j++)
                    data.Add(bt);
            }
            writer.Write(data.ToArray(), 0, data.Count);

            var ratio = (double)baseStream.Position / data.Count;
            Assert.Less(ratio, 0.95);

            baseStream.Position = 0;
            var reader = new CustomCompressionStream(baseStream, true);
            var readData = new List<byte>();
            var buffer = new byte[10];
            while (true)
            {
                int cnt=reader.Read(buffer, 0, buffer.Length);
                readData.AddRange(buffer.Take(cnt));
                if (cnt < buffer.Length) break;
            }
            CollectionAssert.AreEqual(data, readData);
        }

        [Test]
        public void WorkCorrectly()
        {
            for (int i = 0; i < 10; i++)
                Test();
        }
    }
}
