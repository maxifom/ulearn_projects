using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    /// <summary>
    /// Таблица
    /// </summary>
    /// <typeparam name="TRow">
    /// Тип строк
    /// </typeparam>
    /// <typeparam name="TCol">
    /// Тип колонок
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Тип значения ячеек
    /// </typeparam>
    public class Table<TRow, TCol, TValue>
    {
        public List<TRow> Rows = new List<TRow>();
        public List<TCol> Columns = new List<TCol>();
        private TableDictionary<TRow, TableDictionary<TCol, TValue>> tableGrid;

        /// <summary>
        /// Свойство для доступа
        /// к ячейкам таблицы
        /// </summary>
        public Table<TRow, TCol, TValue> Open
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Добавить строку в таблицу
        /// </summary>
        /// <param name="row">
        /// Строка для добавления
        /// </param>
        public void AddRow(TRow row)
        {
            if (!this.RowExists(row))
            {
                this.Rows.Add(row);
            }
        }

        /// <summary>
        /// Добавить столбец в таблицу
        /// </summary>
        /// <param name="column">
        /// Столбец для добавления
        /// </param>
        public void AddColumn(TCol column)
        {
            if (!this.ColExists(column))
            {
                this.Columns.Add(column);
            }
        }

        /// <summary>
        /// Индексатор для доступа
        /// к ячейкам таблицы
        /// с автоматическим созданием 
        /// столбцов и строк при их
        /// несуществовании
        /// </summary>
        /// <param name="row">
        /// Строка
        /// </param>
        /// <param name="col">
        /// Столбец
        /// </param>
        /// <returns>
        /// Ячейку таблицы
        /// </returns>
        public TValue this[TRow row, TCol col]
        {
            get
            {
                return this.tableGrid[row][col];
            }
            set
            {
                if (value.GetType() != typeof(TValue))
                {
                    throw new ArgumentException();
                }
                this.AddRow(row);
                this.AddColumn(col);
                this.tableGrid[row][col] = value;
            }
        }

        /// <summary>
        /// Проверяет строку на
        /// существование
        /// </summary>
        /// <param name="row">
        /// Строка
        /// </param>
        /// <returns>
        /// true - если строка существует
        /// false - если строка не существует
        /// </returns>
        public bool RowExists(TRow row)
        {
            return Rows.IndexOf(row) > -1 && Rows.Count > 0;
        }

        /// <summary>
        /// Проверяет столбец на
        /// существование
        /// </summary>
        /// <param name="col">
        /// Столбец
        /// </param>
        /// <returns>
        /// true - если столбец существует
        /// false - если столбец не существует
        /// </returns>
        public bool ColExists(TCol col)
        {
            return Columns.IndexOf(col) > -1 && Columns.Count > 0;
        }

        /// <summary>
        /// Проверяет столбец
        /// и строку на существование
        /// </summary>
        /// <param name="row">
        /// Строка
        /// </param>
        /// <param name="col">
        /// Столбец
        /// </param>
        /// <returns>
        /// true - если строка и столбец
        /// существуют
        /// false - если строка или столбец
        /// не существуют
        /// </returns>
        public bool Exists(TRow row, TCol col)
        {
            return RowExists(row) && ColExists(col);
        }

        /// <summary>
        /// Свойство для доступа к 
        /// таблице с проверкой
        /// на существование ячейки
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Вызывается при несуществовании 
        /// ячейки
        /// </exception>
        public TableAccessor<TRow, TCol, TValue> Existed
        {
            get
            {
                return new TableAccessor<TRow, TCol, TValue>(this);
            }
        }
    }

    /// <summary>
    /// Словарь для двумерного массивного
    /// доступа к ячейкам таблицы
    /// </summary>
    /// <typeparam name="TRow">
    /// Строка
    /// </typeparam>
    /// <typeparam name="TCol">
    /// Столбец
    /// </typeparam>
    public class TableDictionary<TRow, TCol>
    {
        private Dictionary<TRow, TCol> dictionary = new Dictionary<TRow, TCol>();
        public TCol this[TRow row]
        {
            get
            {
                if (!dictionary.ContainsKey(row))
                {
                    dictionary.Add(row, Activator.CreateInstance<TCol>());
                }
                return dictionary[row];
            }
            set
            {
                dictionary[row] = value;
            }
        }
    }

    /// <summary>
    /// Дает доступ к ячейкам таблицы
    /// с проверкой на существование
    /// </summary>
    /// <typeparam name="TRow">
    /// Тип строки
    /// </typeparam>
    /// <typeparam name="TCol">
    /// Тип столбца
    /// </typeparam>
    /// <typeparam name="TValue">
    /// Тип значения ячейки
    /// </typeparam>
    public class TableAccessor<TRow, TCol, TValue>
    {
        private Table<TRow, TCol, TValue> table;

        /// <summary>
        /// Дает доступ к ячейкам таблицы
        /// с проверкой на существование
        /// </summary>
        /// <param name="row">
        /// Строка
        /// </param>
        /// <param name="col">
        /// Столбец
        /// </param>
        /// <returns>
        /// Ячейку таблицы
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Вызывается при несуществовании 
        /// ячейки
        /// </exception>
        public TValue this[TRow row, TCol col]
        {
            get
            {
                if (!table.Exists(row, col))
                {
                    throw new ArgumentException();
                }

                return table[row, col];
            }
            set
            {
                if (!table.Exists(row, col) || value.GetType() != typeof(TValue))
                {
                    throw new ArgumentException();
                }

                table[row, col] = value;
            }
        }

        /// <summary>
        /// Конструктор класса
        /// для доступа к ячейкам
        /// таблицы
        /// </summary>
        /// <param name="table">
        /// Таблица для доступа
        /// </param>
        public TableAccessor(Table<TRow, TCol, TValue> table)
        {
            this.table = table;
        }
    }
}
