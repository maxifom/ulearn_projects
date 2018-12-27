using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generics.Robots
{
    /// <summary>
    /// Интерфейс для получения 
    /// команды
    /// </summary>
    /// <typeparam name="T">
    /// Команда 
    /// </typeparam>
    public interface IRobotAI<out T>
        where T: IMoveCommand
    {
        /// <summary>
        /// Возвращает команду
        /// </summary>
        /// <returns>
        /// Команду 
        /// </returns>
        T GetCommand();
    }
    
    /// <summary>
    /// Абстрактный класс 
    /// для получения команды
    /// </summary>
    /// <typeparam name="T">
    /// Команда 
    /// </typeparam>
    public abstract class RobotAI<T>: IRobotAI<T>
        where T: IMoveCommand
    {
        protected int counter = 1;

        /// <summary>
        /// Получает команду 
        /// </summary>
        /// <returns>
        /// Команду
        /// </returns>
        public abstract T GetCommand();
    }

    /// <summary>
    /// Искуственный интеллект
    /// для команд стрельбы
    /// </summary>
    public class ShooterAI : RobotAI<ShooterCommand>
    {
        /// <summary>
        /// Возвращает команду
        /// стрельбы
        /// </summary>
        /// <returns>
        /// Команду стрельбы
        /// </returns>
        public override ShooterCommand GetCommand()
        {
            return ShooterCommand.ForCounter(counter++);
        }
    }

    /// <summary>
    /// Искуственный интеллект
    /// для команд постройки
    /// </summary>
    public class BuilderAI : RobotAI<BuilderCommand>
    {
        /// <summary>
        /// Возвращает команду
        /// постройки
        /// </summary>
        /// <returns>
        /// Команду постройки
        /// </returns>
        public override BuilderCommand GetCommand()
        {
            return BuilderCommand.ForCounter(counter++);
        }
    }

    /// <summary>
    /// Интерфейс для 
    /// исполнения команды
    /// </summary>
    /// <typeparam name="T">
    /// Команда  для исполнения
    /// </typeparam>
    public interface IDevice<in T>
        where T: IMoveCommand
    {
        /// <summary>
        /// Выполняет команду
        /// </summary>
        /// <param name="command">
        /// Команда 
        /// </param>
        /// <returns>
        /// Строковое представление
        /// исполненной команды 
        /// </returns>
        string ExecuteCommand(T command);
    }

    /// <summary>
    /// Абстрактный класс
    /// для исполнения команды
    /// </summary>
    /// <typeparam name="T">
    /// Команда для исполнения
    /// </typeparam>
    public abstract class Device<T>:IDevice<T>
        where T: IMoveCommand
    {
        /// <summary>
        /// Выполняет команду 
        /// </summary>
        /// <param name="command">
        /// Команда для исполнения
        /// </param>
        /// <returns>
        /// Строковое представление
        /// исполненной команды 
        /// </returns>
        public abstract string ExecuteCommand(T command);
    }

    /// <summary>
    /// Устройство движения
    /// </summary>
    public class Mover : Device<IMoveCommand>
    {
        /// <summary>
        /// Выполняет команду
        /// движения
        /// </summary>
        /// <param name="command">
        /// Команда движения
        /// </param>
        /// <returns>
        /// Строковое представление 
        /// исполняемой команды движения 
        /// </returns>
        public override string ExecuteCommand(IMoveCommand command)
        {
            if (command == null)
                throw new ArgumentException();
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    /// <summary>
    /// Робот
    /// </summary>
    public class Robot
    {
        IRobotAI<IMoveCommand> ai;
        IDevice<IMoveCommand> device;
        
        /// <summary>
        /// Конструктор робота
        /// </summary>
        /// <param name="ai">
        /// Искуственный интеллект
        /// </param>
        /// <param name="executor">
        /// Устройство исполнения
        /// комманд
        /// </param>
        public Robot(IRobotAI<IMoveCommand> ai, IDevice<IMoveCommand> executor)
        {
            this.ai = ai;
            this.device = executor;
        }

        /// <summary>
        /// Начинает выполнение команд
        /// </summary>
        /// <param name="steps">
        /// Количество шагов
        /// </param>
        /// <returns>
        /// Исполненные команды
        /// </returns>
        public IEnumerable<string> Start(int steps)
        {
             for (int i=0;i<steps;i++)
             {
                 var command = ai.GetCommand();
                 if (command == null)
                     break;
                 yield return device.ExecuteCommand(command);
             }
        }

        /// <summary>
        /// Создает нового робота
        /// </summary>
        /// <param name="ai">
        /// Искуственный интеллект
        /// </param>
        /// <param name="executor">
        /// Устройства для исполнения команд
        /// </param>
        /// <returns>
        /// Нового робота с 
        /// заданным искуственным интеллектом
        /// и устройством исполнения
        /// команд
        /// </returns>
        public static Robot Create(IRobotAI<IMoveCommand> ai, IDevice<IMoveCommand> executor)
        {
            return new Robot(ai, executor);
        }
    }
}
