using System;
using System.Collections.Generic;
namespace Incapsulation.Failures
{
    /// <summary>
    /// Класс для стандартных
    /// функций
    /// </summary>
    public class Common
    {
        /// <summary>
        /// Сравнение двух дат
        /// </summary>
        /// <param name="date1">
        /// Первая дата для сравнения
        /// </param>
        /// <param name="date2">
        /// Вторая дата для сравнения
        /// </param>
        /// <returns>
        /// true - если первая дата раньше второй
        /// false - если первая дата не раньше второй
        /// </returns>
        public static bool IsEarlier(DateTime date1, DateTime date2)
        {
            return DateTime.Compare(date1, date2) < 0;
        }
    }

    /// <summary>
    /// Класс для создания отчетов
    /// по устройствам со сбоями
    /// </summary>
    public class ReportMaker
    {
        /// <summary>
        /// Создание списка устройств со сбоями
        /// раньше выбранной даты
        /// </summary>
        /// <param name="day">День для выборки по дате</param>
        /// <param name="month">Месяц для выборки по дате</param>
        /// <param name="year">Год для выборки по дате</param>
        /// <param name="failureTypes">
        /// Массив сбоев для устройств
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId">
        /// Массив ID для устройств
        /// </param>
        /// <param name="times">
        /// Массив с датами сбоев
        /// </param>
        /// <param name="devices">
        /// Массив устройств
        /// </param>
        /// <returns>
        /// Список устройств со сбоями
        /// раньше выбранной даты
        /// </returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes,
            int[] deviceIds,
            object[][] dates,
            List<Dictionary<string, object>> devicesList)
        {
            var devices = new Device[deviceIds.Length];
            for (var i = 0; i < deviceIds.Length; i++)
            {
                var deviceId = (int)devicesList[i]["DeviceId"];
                var name = (string)devicesList[i]["Name"];
                var failureDate = new DateTime((int)dates[i][2], (int)dates[i][1], (int)dates[i][0]);
                var failure = new Failure(failureTypes[i], failureDate);
                devices[i] = new Device(deviceId, name, failure);
            }
            var date = new DateTime(year, month, day);
            return FindDevicesFailedBeforeDate(date, devices);
        }

        /// <summary>
        /// Создание списка устройств со сбоями
        /// раньше выбранной даты
        /// </summary>
        /// <param name="date">
        /// Дата для выборки устройств со сбоями
        /// </param>
        /// <param name="devices">
        /// Массив устройств со сбоями
        /// </param>
        /// <returns>
        /// Список устройств со сбоями
        /// раньше выбранной даты 
        /// </returns>
        public static List<string> FindDevicesFailedBeforeDate(DateTime date, Device[] devices)
        {
            var problematicDevices = new HashSet<int>();
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].Failure.IsSerious() && Common.IsEarlier(devices[i].Failure.Date, date))
                {
                    problematicDevices.Add(devices[i].DeviceId);
                }
            }
            var result = new List<string>();
            foreach (var device in devices)
            {
                if (problematicDevices.Contains(device.DeviceId))
                {
                    result.Add(device.Name);
                }
            }
            return result;
        }
    }

    /// <summary>
    /// Перечисление для хранения
    /// возможных типов сбоев
    /// </summary>
    enum FailureType : int
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    /// <summary>
    /// Класс для хранения устройства
    /// </summary>
    /// <remarks>
    /// Содержит ID устройства,
    /// имя устройства,
    /// сбой устройства
    /// </remarks>
    public class Device
    {
        public int DeviceId;
        public string Name;
        public Failure Failure;

        /// <summary>
        /// Конструктор устройства
        /// </summary>
        /// <param name="deviceId">
        /// ID устройства
        /// </param>
        /// <param name="name">
        /// Имя устройства
        /// </param>
        /// <param name="failure">
        /// Сбой устройства
        /// </param>
        public Device(int deviceId, string name, Failure failure)
        {
            this.DeviceId = deviceId;
            this.Name = name;
            this.Failure = failure;
        }
    }

    /// <summary>
    /// Класс для хранения сбоя
    /// </summary>
    /// <remarks>
    /// Содержит тип сбоя,
    /// дату ее возникновения
    /// </remarks>
    public class Failure
    {
        public int Type;
        public DateTime Date;

        /// <summary>
        /// Конструктор ошибки
        /// </summary>
        /// <param name="type">
        /// Тип ошибки
        /// </param>
        /// <param name="date">
        /// Дата возникновения ошибки
        /// </param>
        public Failure(int type, DateTime date)
        {
            this.Type = type;
            this.Date = date;
        }

        /// <summary>
        /// Проверяет сбой на серьезность
        /// </summary>
        /// <returns>
        /// true - если сбой серьезный
        /// false - если сбой несерьезный
        /// </returns>
        public bool IsSerious()
        {
            return this.Type == (int)FailureType.UnexpectedShutdown ||
                   this.Type == (int)FailureType.HardwareFailures;
        }
    }
}
