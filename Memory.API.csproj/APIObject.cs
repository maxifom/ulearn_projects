using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.API
{
    /// <summary>
    /// Обертка над API
    /// </summary>
    public class APIObject : IDisposable
    {
        /// <summary>
        /// Число для добавления
        /// в набор API
        /// </summary>
        public int n { get; private set; }

        /// <summary>
        /// Конструктор API
        /// </summary>
        /// <param name="n">
        /// Число для добавления
        /// в набор API
        /// </param>
        public APIObject(int n)
        {
            this.n = n;
            MagicAPI.Allocate(n);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        /// <summary>
        /// Освободить память,
        /// выделенную API
        /// </summary>
        /// <param name="disposing">
        /// Флаг для указания
        /// откуда была вызвана функция:
        /// true - явным образом
        /// false - из финализатора
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                MagicAPI.Free(this.n);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Финализатор API
        /// </summary>
        ~APIObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Освободить память,
        /// выделенную API
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
