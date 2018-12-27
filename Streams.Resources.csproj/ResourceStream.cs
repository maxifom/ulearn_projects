using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Streams.Resources
{
    /// <summary>
    /// Поток для чтения
    /// секции файла
    /// </summary>
    public class ResourceReaderStream : Stream
    {
        private Stream stream;
        private byte[] key;
        private byte[] streamData;
        private IEnumerator data;
        private int valuePosition;
        private static int size = 1024;
        private bool firstTime = true;
        private bool valueEnded = false;

        /// <summary>
        /// Конструктор потока
        /// для чтения секции файла
        /// </summary>
        /// <param name="stream">
        /// Базовый поток
        /// </param>
        /// <param name="key">
        /// Строка для поиска
        /// </param>
        public ResourceReaderStream(Stream stream, string key)
        {
            this.stream = new BufferedStream(stream, size);
            this.key = Encoding.ASCII.GetBytes(key);

            this.streamData = new byte[size];
            this.data = streamData.GetEnumerator();
        }

        /// <summary>
        /// Находит заданную
        /// строку в файле
        /// </summary>
        private void SeekKey()
        {
            int i = 0;
            int currentPosition = 0;
            valuePosition = -1;
            while (data.MoveNext())
            {
                if (i < key.Length)
                {
                    if ((byte)data.Current == key[i])
                    {
                        i++;
                    }
                    else i = 0;
                }
                else
                {
                    valuePosition = currentPosition + 2;
                    data.MoveNext();
                    break;
                }

                currentPosition++;
            }
        }

        /// <summary>
        /// Cчитывает последовательность байтов
        /// из потока, находит заданную строку
        /// и перемещает позицию в потоке
        /// на число считанных байтов 
        /// </summary>
        /// <param name="buffer">
        /// Буфер для чтения
        /// </param>
        /// <param name="offset">
        /// Смещение в буфере
        /// </param>
        /// <param name="count">
        /// Количество считываемых байт
        /// </param>
        /// <returns>
        /// Количество байт,
        /// считанных в буфер
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (firstTime)
            {
                stream.Read(streamData, offset, size);
                SeekKey();
                firstTime = false;
            }
            if (valueEnded || this.valuePosition<=0)
                return 0;
            data.MoveNext();
            return ReadData(buffer, offset);
        }

        /// <summary>
        /// Читает поток с заданного места
        /// и возвращает количество 
        /// считанных байт
        /// </summary>
        /// <param name="buffer">
        /// Буфер для чтения
        /// </param>
        /// <param name="offset">
        /// Смещение в потоке
        /// </param>
        /// <returns>
        /// Количество считанных байт
        /// </returns>
        private int ReadData(byte[] buffer, int offset)
        {
            int i = 0;
            int bufferOffset = 0;
            byte lastValue = 1;
            while ((byte)data.Current != 1 || lastValue != 0)
            {
                buffer[i + bufferOffset] = (byte)data.Current;
                if (lastValue == 0 && (byte)data.Current == 0)
                    bufferOffset--;
                lastValue = (byte)data.Current;
                i++;
                if (i + bufferOffset == buffer.Length - 1)
                    break;
                if (!data.MoveNext())
                    ReadFromStream(offset);
                if ((byte)data.Current == 1 && lastValue == 0)
                {
                    valueEnded = true;
                    i--;
                }
            }
            return i + bufferOffset;
        }

        /// <summary>
        /// Читает следующее количество
        /// байт из потока
        /// </summary>
        /// <param name="offset">
        /// Смещение в потоке
        /// </param>
        public void ReadFromStream(int offset)
        {
            this.stream.Read(this.streamData, offset, ResourceReaderStream.size);
            this.data = this.streamData.GetEnumerator();
            this.data.MoveNext();
        }

        public override void Flush() => throw new NotImplementedException();

        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotImplementedException();

        public override void SetLength(long value) =>
            throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) =>
            throw new NotImplementedException();

        public override bool CanRead => throw new NotImplementedException();

        public override bool CanSeek => throw new NotImplementedException();

        public override bool CanWrite => throw new NotImplementedException();

        public override long Length => throw new NotImplementedException();

        public override long Position
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}