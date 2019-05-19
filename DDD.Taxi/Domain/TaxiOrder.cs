using System;
using System.Globalization;
using System.Linq;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
    /// <summary>
    /// Репозиторий водителей
    /// Оставлен для работы тестов
    /// </summary>
    public class DriversRepository
    {
    }

    /// <summary>
    /// Интерфейс для работы с заказом в такси
    /// </summary>
    public class TaxiApi : ITaxiApi<TaxiOrder>
    {
        private readonly DriversRepository driversRepo;
        private readonly Func<DateTime> currentTime;
        private int idCounter;

        public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
        {
            this.driversRepo = driversRepo;
            this.currentTime = currentTime;
        }

        /// <summary>
        /// Создает заказ без места назначения
        /// </summary>
        /// <param name="firstName">
        /// Имя клиента
        /// </param>
        /// <param name="lastName">
        /// Фамилия клиента
        /// </param>
        /// <param name="street">
        /// Улица
        /// </param>
        /// <param name="building">
        /// Здание
        /// </param>
        /// <returns>
        /// Новый заказ без места назначения
        /// </returns>
        public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName,
                                                       string street, string building)
        {
            return
                new TaxiOrder(
                    idCounter++,
                    new PersonName(firstName, lastName),
                    new Address(street, building),
                    currentTime()
                );
        }

        /// <summary>
        /// Обновить место назначения
        /// </summary>
        /// <param name="order">
        /// Заказ для обновления
        /// </param>
        /// <param name="street">
        /// Улица
        /// </param>
        /// <param name="building">
        /// Здание
        /// </param>
        public void UpdateDestination(TaxiOrder order, string street, string building)
        {
            order.UpdateDestination(new Address(street, building));
        }

        /// <summary>
        /// Назначить водителя к заказу
        /// </summary>
        /// <param name="order">
        /// Заказ для назначения водителя
        /// </param>
        public void AssignDriver(TaxiOrder order, int driverId)
        {
            order.AssignDriver(driverId, currentTime());
        }

        /// <summary>
        /// Отменить назначение водителя к заказу
        /// </summary>
        /// <param name="order">
        /// Заказ для отмены назначения водителя
        /// </param>
        public void UnassignDriver(TaxiOrder order)
        {
            order.UnassignDriver();
        }

        /// <summary>
        /// Получить информацию о водителе заказа
        /// </summary>
        /// <param name="order">
        /// Заказ для получения информации о водителе
        /// </param>
        /// <returns>
        /// Информацию о водителе заказа
        /// </returns>
        public string GetDriverFullInfo(TaxiOrder order)
        {
            return order.GetDriverFullInfo();
        }

        /// <summary>
        /// Получить краткую информацию о заказе
        /// </summary>
        /// <param name="order">
        /// Заказ для получения информации
        /// </param>
        /// <returns>
        /// Краткую информацию о заказе
        /// </returns>
        public string GetShortOrderInfo(TaxiOrder order)
        {
            return order.GetShortOrderInfo();
        }

        /// <summary>
        /// Получает время последнего статуса заказа
        /// </summary>
        /// <param name="order">
        /// Заказ для получения статуса
        /// </param>
        /// <returns>
        /// Время последнего статуса заказа
        /// </returns>
        private DateTime GetLastProgressTime(TaxiOrder order)
        {
            return order.GetLastProgressTime();
        }

        /// <summary>
        /// Отменить заказ
        /// </summary>
        /// <param name="order">
        /// Заказ для отмены
        /// </param>
        public void Cancel(TaxiOrder order)
        {
            order.Cancel(currentTime());
        }

        /// <summary>
        /// Начать поездку
        /// </summary>
        /// <param name="order">
        /// Заказ для начала поездки
        /// </param>
        public void StartRide(TaxiOrder order)
        {
            order.StartRide(currentTime());
        }

        /// <summary>
        /// Завершить поездку
        /// </summary>
        /// <param name="order">
        /// Заказ для завершения поездки
        /// </param>
        public void FinishRide(TaxiOrder order)
        {
            order.FinishRide(currentTime());
        }
    }

    /// <summary>
    /// Заказ такси
    /// </summary>
    public class TaxiOrder : Entity<int>
    {
        // Номер заказа
        private readonly int id;
        // Полное имя клиента
        public PersonName ClientName { get; private set; }
        // Адрес начальной точки заказа
        public Address Start { get; private set; }
        // Адрес конечной точки заказа
        public Address Destination { get; private set; }
        // Водитель такси для заказа
        public Driver Driver { get; private set; }
        // Статус заказа
        public TaxiOrderStatus Status { get; private set; }
        // Дата создания заказа
        public DateTime CreationTime { get; private set; }
        // Дата назначения водителя
        public DateTime DriverAssignmentTime { get; private set; }
        // Дата отмены заказа
        public DateTime CancelTime { get; private set; }
        // Дата начала поездки
        public DateTime StartRideTime { get; private set; }
        // Дата конца поездки
        public DateTime FinishRideTime { get; private set; }

        public TaxiOrder(int id, PersonName clientName, Address startAddress, DateTime dateTime) : base(id)
        {
            this.id = id;
            ClientName = clientName;
            Start = startAddress;
            CreationTime = dateTime;
        }

        /// <summary>
        /// Отменить заказ
        /// </summary>
        /// <param name="cancelTime">
        /// Время отмены заказа
        /// </param>
        public void Cancel(DateTime cancelTime)
        {
            if (Status == TaxiOrderStatus.InProgress)
                throw new InvalidOperationException();
            Status = TaxiOrderStatus.Canceled;
            CancelTime = cancelTime;
        }

        /// <summary>
        /// Обновить место назначения
        /// </summary>
        /// <param name="destination">
        /// Новое место назначения
        /// </param>
        public void UpdateDestination(Address destination)
        {
            Destination = destination;
        }

        /// <summary>
        /// Назначить водителя к текущему заказу
        /// </summary>
        /// <param name="driverId">
        /// Номер водителя
        /// </param>
        /// <param name="assignTime">
        /// Время назначения
        /// </param>
        public void AssignDriver(int driverId, DateTime assignTime)
        {
            if (Driver == null)
            {
                if (driverId == 15)
                {
                    Driver = new Driver(driverId, new PersonName("Drive", "Driverson"),
                                       "Lada sedan", "Baklazhan", "A123BT 66");
                    DriverAssignmentTime = assignTime;
                    Status = TaxiOrderStatus.WaitingCarArrival;
                }
                else
                    throw new Exception("Unknown driver id " + driverId);
            }
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Отменить назначение водителя к текущему заказу
        /// </summary>
        public void UnassignDriver()
        {
            if (Status == TaxiOrderStatus.InProgress || Driver == null)
                throw new InvalidOperationException(Status.ToString());
            Driver = new Driver(null, null, null, null, null);
            Status = TaxiOrderStatus.WaitingForDriver;
        }

        /// <summary>
        /// Начать поездку
        /// </summary>
        /// <param name="startTime">
        /// Время начала поездки
        /// </param>
        public void StartRide(DateTime startTime)
        {
            if (Driver == null)
                throw new InvalidOperationException();
            Status = TaxiOrderStatus.InProgress;
            StartRideTime = startTime;
        }

        /// <summary>
        /// Закончить поездку
        /// </summary>
        /// <param name="finishTime">
        /// Время окончания поездки
        /// </param>
        public void FinishRide(DateTime finishTime)
        {
            if (Status != TaxiOrderStatus.InProgress || Driver == null)
                throw new InvalidOperationException();
            Status = TaxiOrderStatus.Finished;
            FinishRideTime = finishTime;
        }

        /// <summary>
        /// Получить информацию о водителе заказа
        /// </summary>
        /// <returns>
        /// Информацию о водителе заказа
        /// </returns>
        public string GetDriverFullInfo()
        {
            if (Status == TaxiOrderStatus.WaitingForDriver)
                return null;
            return string.Join(" ",
                "Id: " + Driver.Identificator,
                "DriverName: " + FormatName(Driver.Name.FirstName, Driver.Name.LastName),
                "Color: " + Driver.Car.Color,
                "CarModel: " + Driver.Car.Model,
                "PlateNumber: " + Driver.Car.PlateNumber);
        }

        /// <summary>
        /// Форматирует полное имя в строку
        /// </summary>
        /// <param name="firstName">
        /// Имя
        /// </param>
        /// <param name="lastName">
        /// Фамилия
        /// </param>
        /// <returns>
        /// Строковое представление полного имени
        /// </returns>
        private static string FormatName(string firstName, string lastName)
        {
            return string.Join(" ", new[] { firstName, lastName }.Where(n => n != null));
        }

        /// <summary>
        /// Форматирует адрес в строку
        /// </summary>
        /// <param name="street">
        /// Улица
        /// </param>
        /// <param name="building">
        /// Здание
        /// </param>
        /// <returns>
        /// Строковое представление адреса
        /// </returns>
        private static string FormatAddress(string street, string building)
        {
            return string.Join(" ", new[] { street, building }.Where(n => n != null));
        }

        /// <summary>
        /// Получить краткую информацию о заказе
        /// </summary>
        /// <returns>
        /// Краткую информацию о заказе
        /// </returns>
        public string GetShortOrderInfo()
        {
            var gotDriver = Driver == null || Driver.Name == null;
            var driver = gotDriver ? "" : FormatName(Driver.Name.FirstName, Driver.Name.LastName);
            var destination = Destination == null ? "" :
                              FormatAddress(Destination.Street, Destination.Building);
            return string.Join(" ",
                "OrderId: " + id,
                "Status: " + Status,
                "Client: " + FormatName(ClientName.FirstName, ClientName.LastName),
                "Driver: " + driver,
                "From: " + FormatAddress(Start.Street, Start.Building),
                "To: " + destination,
                "LastProgressTime: " + GetLastProgressTime()
                                        .ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Получает время последнего статуса заказа
        /// </summary>
        /// <returns>
        /// Время последнего статуса заказа
        /// </returns>
        public DateTime GetLastProgressTime()
        {
            switch (Status)
            {
                case TaxiOrderStatus.WaitingForDriver:
                    return CreationTime;
                case TaxiOrderStatus.WaitingCarArrival:
                    return DriverAssignmentTime;
                case TaxiOrderStatus.InProgress:
                    return StartRideTime;
                case TaxiOrderStatus.Finished:
                    return FinishRideTime;
                case TaxiOrderStatus.Canceled:
                    return CancelTime;
            }
            throw new NotSupportedException(Status.ToString());
        }
    }

    /// <summary>
    /// Водитель такси
    /// </summary>
    public class Driver : Entity<int>
    {
        // Номер водителя
        public int? Identificator { get; private set; }
        // Полное имя водителя
        public PersonName Name { get; private set; }
        // Машина водителя
        public Car Car { get; private set; }

        public Driver(int? id, PersonName name, string carModel,
                      string carColor, string carPlateNumber) : base(0)
        {
            this.Identificator = id;
            this.Name = name;
            this.Car = new Car(carModel, carColor, carPlateNumber);
        }
    }

    /// <summary>
    /// Машина
    /// </summary>
    public class Car : ValueType<Car>
    {
        // Модель машины
        public string Model { get; private set; }
        // Цвет машины
        public string Color { get; private set; }
        // Номер машины
        public string PlateNumber { get; private set; }

        public Car(string model, string color, string plateNumber)
        {
            Model = model;
            Color = color;
            PlateNumber = plateNumber;
        }
    }
}
