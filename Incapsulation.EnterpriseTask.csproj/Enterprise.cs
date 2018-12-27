using System;
using System.Linq;
namespace Incapsulation.EnterpriseTask
{
    /// <summary>
    /// Класс для хранения 
    /// информации о компании
    /// </summary>
    /// <remarks>
    /// Содержит уникальный идентификатор,
    /// имя, ИНН, дату основания,
    /// время работы и сумму
    /// проведенных сделок
    /// компании
    /// </remarks>
    public class Enterprise
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public readonly Guid guid;

        /// <summary>
        /// Свойство для получения
        /// уникального идентификатора
        /// </summary>
        public Guid Guid
        {
            get { return guid; }
        }

        /// <summary>
        /// Конструктор компании
        /// </summary>
        /// <param name="guid">
        /// Уникальный идентификатор
        /// компании
        /// </param>
        public Enterprise(Guid guid)
        {
            this.guid = guid;
        }

        /// <summary>
        /// Имя компании
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ИНН компании
        /// </summary>
        public string Inn
        {
            get
            {
                return Inn;
            }
            set
            {
                if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
                    throw new ArgumentException();
                Inn = value;
            }
        }

        /// <summary>
        /// Дата основания компании
        /// </summary>
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// Время работы компании
        /// </summary>
        public TimeSpan ActiveTimeSpan
        {
            get
            {
                return DateTime.Now - EstablishDate;
            }
        }

        /// <summary>
        /// Метод для получения суммы
        /// проведенных сделок компании
        /// </summary>
        /// <returns>
        /// Сумма проведенных
        /// сделок компании
        /// </returns>
        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = 0.0;
            foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == guid))
                amount += t.Amount;
            return amount;
        }
    }
}
