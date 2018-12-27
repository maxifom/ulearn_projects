using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Robots
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public interface IMoveCommand
    {
        Point Destination { get; }
    }

    public class ShooterCommand : IMoveCommand
    {
        public Point Destination
        {
            get;  set;
        }

        public bool Shoot { get; set; }

        public static ShooterCommand ForCounter(int counter)
        {
            return new ShooterCommand
            {
                Destination = new Point { X = counter * 2, Y = counter * 3 },
                Shoot = counter % 5 == 0
            };
        }
    }

    public class BuilderCommand : IMoveCommand
    {
        public Point Destination { get; set; }
        public bool Build { get; set; }

        public static BuilderCommand ForCounter(int counter)
        {
            return new BuilderCommand
            {
                Destination = new Point { X = counter, Y = counter },
                Build = counter % 10 == 0
            };
        }
    }
}
