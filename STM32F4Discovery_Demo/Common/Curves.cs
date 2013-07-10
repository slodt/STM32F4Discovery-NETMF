using System;

namespace Common
{
    public class Point
    {
        public double X;
        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
            X = 0.0;
            Y = 0.0;
        }
    }

    public class Curves
    {

        public static Point GetBezierPoint(double t, Point[] points)
        {
            switch (points.Length)
            {
                case 0:
                    throw new Exception("Invalid points specified");
                case 1:
                    return points[0];
                case 2:
                    return GetBezierPoint(t, points[0], points[0], points[1], points[1]);
                case 3:
                    return GetBezierPoint(t, points[0], points[1], points[1], points[2]);
                default:
                    return GetBezierPoint(t, points, 0, points.Length);
            }
        }

        public static Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count <= 1)
                return controlPoints[index];
            var p0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var p1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * p0.X + t * p1.X, (1 - t) * p0.Y + t * p1.Y);
        }

        public static Point GetBezierPoint(double t, Point p0, Point p1)
        {
            return GetBezierPoint(t, p0, p0, p1, p1);
        }

        public static Point GetBezierPoint(double t, Point p0, Point p1, Point p2)
        {
            return GetBezierPoint(t, p0, p1, p1, p2);
        }

        // CurrentPoint(t, EndPoint1, ControlPoint1, ControlPoint2, EndPoint2)
        public static Point GetBezierPoint(double t, Point p0, Point p1, Point p2, Point p3)
        {
            var cube = t * t * t;
            var square = t * t;

            var ax = 3 * (p1.X - p0.X);
            var ay = 3 * (p1.Y - p0.Y);

            var bx = 3 * (p2.X - p1.X) - ax;
            var by = 3 * (p2.Y - p1.Y) - ay;

            var cx = p3.X - p0.X - ax - bx;
            var cy = p3.Y - p0.Y - ay - by;

            var x = (cx * cube) + (bx * square) + (ax * t) + p0.X;
            var y = (cy * cube) + (by * square) + (ay * t) + p0.Y;

            return new Point(x, y);
        }
    }
}
