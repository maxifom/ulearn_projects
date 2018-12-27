using System;
using System.Collections.Generic;
using System.Text;

namespace GeometryTasks
{
    public class Vector
    {
        public double X;
        public double Y;
        public Vector()
        {
            X = 0;
            Y = 0;
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector vector)
        {
            return Geometry.Add(this, vector);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    public class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            var x1 = segment.Begin.X;
            var x2 = segment.End.X;
            var y1 = segment.Begin.Y;
            var y2 = segment.End.Y;
            return Math.Sqrt((x2-x1) * (x2-x1) + (y2-y1) * (y2-y1));
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            var x = vector.X;
            var y = vector.Y;
            var x1 = segment.Begin.X;
            var x2 = segment.End.X;
            var y1 = segment.Begin.Y;
            var y2 = segment.End.Y;
            bool isInXAndYRange = (x1 <= x && x <= x2 && y1 <= y && y <= y2);
            bool isOnTheSameLine = Math.Abs((x - x1) * (y2 - y1) - (y - y1) * (x2 - x1)) < 0.00001;
            return isInXAndYRange && isOnTheSameLine;
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector vector)
        {
            return Geometry.IsVectorInSegment(vector, this);
        }
    }
}
