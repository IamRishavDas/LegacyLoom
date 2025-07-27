
namespace LegacyLoom.Tests
{
    public class Point {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double getDistance(Point p)
        {
            return Math.Sqrt(Math.Pow(this.X - p.X, 2) + Math.Pow(this.Y - p.Y, 2) + Math.Pow(this.Z - p.Z, 2));
        }
    }

    public class Program
    {
       public static void Main(string[] args)
       {
            Point p1 = new(1, 2);
            Point p2 = new(1, 1);
            Console.Write(p1.getDistance(p2));
       }
    }
}
