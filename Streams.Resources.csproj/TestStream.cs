using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streams.Resources
{
    class TestStream : Stream, IDisposable
    {
        List<int> counts = new List<int>();
        byte[] data;
        int pointer = 0;

        public TestStream(IEnumerable<string> keysAndValues)
        {
            List<byte> bytes = new List<byte>();
            foreach(var e in keysAndValues)
            {
                foreach(var b in Encoding.ASCII.GetBytes(e))
                {
                    if (b == 0) bytes.Add(0);
                    bytes.Add(b);
                }
                bytes.Add(0);
                bytes.Add(1);
            }
            data = bytes.ToArray();
        }

        public IEnumerable<int> Counts {  get { return counts; } }

        public override int Read(byte[] buffer, int offset, int count)
        {
            counts.Add(count);
            count = Math.Min(count, data.Length - pointer);
            for (int i = 0; i < count; i++)
                buffer[i + offset] = data[i+pointer];
            var str = System.Text.Encoding.ASCII.GetString(buffer);
            pointer += count;
            return count;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
