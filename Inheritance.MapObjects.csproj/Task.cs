using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    /// <summary>
    /// Интерфейс для объекта
    /// у которого есть
    /// владелец
    /// </summary>
    /// <remarks>
    /// Owner - ID владельца
    /// </remarks>
    public interface IOwnable
    {
        int Owner { get; set; }
    }

    /// <summary>
    /// Интерфейс для объекта
    /// у которого есть армия
    /// </summary>
    /// <remarks>
    /// Army - армия объекта
    /// </remarks>
    public interface IArmed
    {
        Army Army { get; set; }
    }

    /// <summary>
    /// Интерфейс для объекта
    /// у которого есть награда
    /// </summary>
    /// <remarks>
    /// Treasure - награда объекта
    /// </remarks>
    public interface IGotTreasure
    {
        Treasure Treasure { get; set; }
    }

    /// <summary>
    /// Строение
    /// </summary>
    /// <remarks>
    /// Имеет владельца
    /// </remarks>
    public class Dwelling : IOwnable
    {
        public int Owner { get; set; }
    }

    /// <summary>
    /// Шахта
    /// </summary>
    /// <remarks>
    /// Имеет владельца,
    /// армию и награду
    /// </remarks>
    public class Mine : IOwnable, IArmed, IGotTreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    /// <summary>
    /// Нейтральные существа
    /// </summary>
    /// <remarks>
    /// Имеют армию и награду
    /// </remarks>
    public class Creeps : IArmed, IGotTreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    /// <summary>
    /// Волки
    /// </summary>
    /// <remarks>
    /// Имеют армию
    /// </remarks>
    public class Wolfs : IArmed
    {
        public Army Army { get; set; }
    }

    /// <summary>
    /// Залежи ресурсов
    /// </summary>
    /// <remarks>
    /// Имеют награду
    /// </remarks>
    public class ResourcePile : IGotTreasure
    {
        public Treasure Treasure { get; set; }
    }

    /// <summary>
    /// Класс для взаимодействия
    /// игрока с объектами карты
    /// </summary>
    public static class Interaction
    {
        /// <summary>
        /// Производит взаимодействие 
        /// игрока с объектом карты
        /// </summary>
        /// <param name="player">
        /// Игрок
        /// </param>
        /// <param name="mapObject">
        /// Объект карты
        /// </param>
        public static void Make(Player player, object mapObject)
        {
            if (!(mapObject is IArmed) || player.CanBeat(((IArmed)mapObject).Army))
            {
                if (mapObject is IGotTreasure)
                {
                    player.Consume(((IGotTreasure)mapObject).Treasure);
                }
                if (mapObject is IOwnable)
                {
                    ((IOwnable)mapObject).Owner = player.Id;
                }
            }
            else
            {
                player.Die();
            }
        }
    }
}
