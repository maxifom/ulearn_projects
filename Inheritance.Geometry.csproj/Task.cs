using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.Geometry
{
    /// <summary>
    /// Интерфейс посетителя
    /// для работы над шаром,
    /// кубом и цилиндром
    /// </summary>
    public interface IVisitor
    {
        /// <summary>
        /// Посетить шар для рассчетов
        /// </summary>
        /// <param name="ball">
        /// Шар для рассчетов
        /// </param>
        void Visit(Ball ball);
        /// <summary>
        /// Посетить куб для рассчетов
        /// </summary>
        /// <param name="cube">
        /// Куб для рассчетов
        /// </param>
        void Visit(Cube cube);
        /// <summary>
        /// Посетить цилиндр для рассчетов
        /// </summary>
        /// <param name="cyllinder">
        /// Цилиндр для рассчетов
        /// </param>
        void Visit(Cyllinder cyllinder);
    }

    /// <summary>
    /// Абстрактный класс 
    /// геометрического тела
    /// </summary>
    /// <remarks>
    /// Имеет объем
    /// </remarks>
    public abstract class Body
    {
        /// <summary>
        /// Абстрактный метод
        /// нахождения объема
        /// геометрического тела
        /// </summary>
        /// <returns>
        /// Объем геометрического тела
        /// </returns>
        public abstract double GetVolume();

        /// <summary>
        /// Принять посетителя
        /// для работы над геометрическим
        /// телом
        /// </summary>
        /// <param name="visitor">
        /// Посетитель для работы
        /// над геометрическим телом
        /// </param>
        public abstract void Accept(IVisitor visitor);
    }

    /// <summary>
    /// Шар
    /// </summary>
    /// <remarks>
    /// Имеет радиус и объем
    /// </remarks>
    public class Ball : Body
    {
        public double Radius { get; set; }
        
        /// <summary>
        /// Считает объем шара
        /// </summary>
        /// <returns>
        /// Объем шара
        /// </returns>
        public override double GetVolume()
        {
            return 4.0 * Math.PI * Radius * Radius * Radius / 3;
        }

        /// <summary>
        /// Принять посетителя
        /// для работы над шаром
        /// </summary>
        /// <param name="visitor">
        /// Посетитель для работы
        /// над шаром
        /// </param>
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// Куб
    /// </summary>
    /// <remarks>
    /// Имеет длину ребра
    /// и объем
    /// </remarks>
    public class Cube : Body
    {
        public double Size { get; set; }

        /// <summary>
        /// Считает объем куба
        /// </summary>
        /// <returns>
        /// Объем куба
        /// </returns>
        public override double GetVolume()
        {
            return Size * Size * Size;
        }

        /// <summary>
        /// Принять посетителя
        /// для работы над кубом
        /// </summary>
        /// <param name="visitor">
        /// Посетитель для работы
        /// над кубом
        /// </param>
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    /// <summary>
    /// Цилиндр
    /// </summary>
    /// <remarks>
    /// Имеет высоту, радиус
    /// и объем
    /// </remarks>
    public class Cyllinder : Body
    {
        public double Height { get; set; }
        public double Radius { get; set; }

        /// <summary>
        /// Считает объем цилиндра
        /// </summary>
        /// <returns>
        /// Объем цилиндра
        /// </returns>
        public override double GetVolume()
        {
            return Math.PI * Radius * Radius * Height;
        }

        /// <summary>
        /// Принять посетителя
        /// для работы над цилиндром
        /// </summary>
        /// <param name="visitor">
        /// Посетитель для работы
        /// над цилиндром
        /// </param>
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class SurfaceAreaVisitor : IVisitor
    {
        public double SurfaceArea { get; private set; }

        /// <summary>
        /// Посетить шар для рассчета
        /// площади поверхности
        /// </summary>
        /// <param name="ball">
        /// Шар для рассчетов
        /// </param>
        public void Visit(Ball ball)
        {
            this.SurfaceArea = 4 * Math.PI * ball.Radius * ball.Radius;
        }

        /// <summary>
        /// Посетить куб для рассчета
        /// площади поверхности
        /// </summary>
        /// <param name="cube">
        /// Куб для рассчетов
        /// </param>
        public void Visit(Cube cube)
        {
            this.SurfaceArea = 6 * cube.Size * cube.Size;
        }

        /// <summary>
        /// Посетить цилиндр для рассчета
        /// площади поверхности
        /// </summary>
        /// <param name="cyllinder">
        /// Цилиндр для рассчетов
        /// </param>
        public void Visit(Cyllinder cyllinder)
        {
            this.SurfaceArea = 2 * Math.PI * cyllinder.Radius * (cyllinder.Radius + cyllinder.Height);
        }
    }

    public class DimensionsVisitor : IVisitor
    {
        public Dimensions Dimensions { get; private set; }

        /// <summary>
        /// Посетить шар для рассчета
        /// ширины и высоты
        /// </summary>
        /// <param name="ball">
        /// Шар для рассчетов
        /// </param>
        public void Visit(Ball ball)
        {
            var size = 2 * ball.Radius;
            this.Dimensions = new Dimensions(size, size);
        }

        /// <summary>
        /// Посетить куб для рассчета
        /// ширины и высоты
        /// </summary>
        /// <param name="cube">
        /// Куб для рассчетов
        /// </param>
        public void Visit(Cube cube)
        {
            this.Dimensions = new Dimensions(cube.Size, cube.Size);
        }

        /// <summary>
        /// Посетить цилиндр для рассчета
        /// ширины и высоты
        /// </summary>
        /// <param name="cyllinder">
        /// Цилиндр для рассчетов
        /// </param>
        public void Visit(Cyllinder cyllinder)
        {
            this.Dimensions = new Dimensions(cyllinder.Radius * 2, cyllinder.Height);
        }
    }
}
