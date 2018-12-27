using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Streams.Compression
{
    /// <summary>
    /// Поток для сжатия
    /// </summary>
    public class CustomCompressionStream : Stream
    {
        private MemoryStream stream;
        private bool readOnly;
        private static int size = 1024;
        private int pointer = 0;
        private List<byte> data = new List<byte>();

        /// <summary>
        /// Конструктор потока сжатия
        /// </summary>
        /// <param name="baseStream">
        /// Базовый поток
        /// </param>
        /// <param name="readOnly">
        /// Флаг "только для чтения"
        /// </param>
        public CustomCompressionStream(MemoryStream baseStream, bool readOnly)
        {
            this.stream = baseStream;
            this.readOnly = readOnly;
        }

        /// <summary>
        /// Флаг для обозначения
        /// возможности чтения из потока
        /// </summary>
        public override bool CanRead => stream.CanRead;

        /// <summary>
        /// Флаг для обозначения
        /// возможности поиска в потоке
        /// </summary>
        public override bool CanSeek => stream.CanSeek && !readOnly;

        /// <summary>
        /// Флаг для обозначения
        /// возможности записи в поток
        /// </summary>
        public override bool CanWrite => stream.CanWrite && !readOnly;

        /// <summary>
        /// Длина потока
        /// </summary>
        public override long Length =>
                throw new NotImplementedException();

        /// <summary>
        /// Позиция в потоке
        /// </summary>
        public override long Position
        {
            get =>
                throw new NotImplementedException();

            set =>
                throw new NotImplementedException();
        }

        /// <summary>
        /// Чистит буфер и сохраняет изменения
        /// потока
        /// </summary>
        public override void Flush() => stream.Flush();

        /// <summary>
        /// Задает позицию в потоке
        /// </summary>
        /// <param name="offset">
        /// Смещение в байтах
        /// </param>
        /// <param name="origin">
        /// Точка ссылки, которая используется
        /// для получения новой позиции
        /// </param>
        /// <returns>
        /// Новую позицию в потоке
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Устанавливает длину потока
        /// </summary>
        /// <param name="value">
        /// Длина потока
        /// </param>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сжимает и записывает последовательность байтов
        /// в поток и перемещает текущую позицию
        /// в нем вперед на число записанных байтов
        /// </summary>
        /// <param name="buffer">
        /// Буфер для сжатия и записи
        /// </param>
        /// <param name="offset">
        /// Смещение в буфере 
        /// </param>
        /// <param name="count">
        /// Количество записываемых байт
        /// </param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            var compressedData = Compress(buffer.ToList());
            stream.Write(compressedData, offset, compressedData.Count());
        }

        /// <summary>
        /// Читает и распоковывает последовательность
        /// байтов из текущего потока 
        /// к нормальному виду и перемещает позицию
        /// в потоке на число считанных байтов
        /// </summary>
        /// <param name="buffer">
        /// Буфер для чтения
        /// </param>
        /// <param name="offset">
        /// Смещение в буфере
        /// </param>
        /// <param name="count">
        /// Число считываемых байт
        /// </param>
        /// <returns>
        /// Общее количество байтов,
        /// считанных в буфер
        /// </returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var endOfStream = false;
            while (!endOfStream)
            {
                byte[] buff = new byte[size];
                var readCount = stream.Read(buff, 0, size);
                foreach (var element in Decompress(buff.Take(readCount).ToArray()))
                    data.Add(element);
                if (readCount == 0)
                    endOfStream = true;
            }
            var copyCount = 0;
            if (this.pointer + count < data.Count)
                copyCount = count;
            else
                copyCount = data.Count - pointer;
            data.CopyTo(pointer, buffer, 0, copyCount);
            pointer += copyCount;
            return copyCount;
        }


        /// <summary>
        /// Распаковывает буфер
        /// </summary>
        /// <param name="buffer">
        /// Буфер для распаковки
        /// </param>
        /// <returns>
        /// Распакованный буфер
        /// </returns>
        public byte[] Decompress(byte[] buffer)
        {
            var decompressedData = new List<byte>();
            var i = 0;
            var j = 0;
            while (i < buffer.Length - 1)
            {
                while (j < buffer[i + 1])
                {
                    decompressedData.Add(buffer[i]);
                    j++;
                }
                i += 2;
                j = 0;
            }
            return decompressedData.ToArray();
        }

        /// <summary>
        /// Сжимает данных
        /// </summary>
        /// <param name="data">
        /// Данные для сжатия
        /// </param>
        /// <returns>
        /// Данные в сжатом виде
        /// </returns>
        public byte[] Compress(List<byte> data)
        {
            var repeat = 1;
            var i = 0;
            var compressedData = new List<byte>();
            while (i < data.Count)
            {
                while (i < data.Count - 1 && data[i] == data[i + 1])
                {
                    i++;
                    repeat++;
                    if (repeat >= 255)
                        break;
                }
                compressedData.Add(data[i]);
                compressedData.Add(Convert.ToByte(repeat));
                repeat = 1;
                i++;
            }
            return compressedData.ToArray();
        }
    }
}